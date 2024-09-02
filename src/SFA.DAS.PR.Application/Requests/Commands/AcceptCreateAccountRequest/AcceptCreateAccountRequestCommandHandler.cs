using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Application.Permissions.Commands.PostPermissions;
using SFA.DAS.PR.Data;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.Types.Models;
using System.Text.Json;

namespace SFA.DAS.PR.Application.Requests.Commands.AcceptCreateAccountRequest;

public class AcceptCreateAccountRequestCommandHandler(
    ILogger<AcceptCreateAccountRequestCommandHandler> _logger,
    IProviderRelationshipsDataContext _providerRelationshipsDataContext,
    IAccountWriteRepository _accountWriteRepository,
    IAccountReadRepository _accountReadRepository,
    IAccountProviderWriteRepository _accountProviderWriteRepository,
    IAccountLegalEntityReadRepository _accountLegalEntityReadRepository,
    IAccountLegalEntityWriteRepository _accountLegalEntityWriteRepository,
    IAccountProviderLegalEntitiesWriteRepository _accountProviderLegalEntitiesWriteRepository,
    IPermissionsWriteRepository _permissionsWriteRepository,
    IPermissionsAuditWriteRepository _permissionsAuditWriteRepository,
    IMessageSession _messageSession
) : IRequestHandler<AcceptCreateAccountRequestCommandWrapper, ValidatedResponse<AcceptCreateAccountRequestCommandResult>>
{
    public async Task<ValidatedResponse<AcceptCreateAccountRequestCommandResult>> Handle(AcceptCreateAccountRequestCommandWrapper wrapper, CancellationToken cancellationToken)
    {
        Request request = await _providerRelationshipsDataContext.Requests
                                    .Include(a => a.PermissionRequests)
                                    .FirstAsync(a => a.Id == wrapper.RequestId, cancellationToken);

        await CreateAccountAndAddPermissions(wrapper.Commmand, request, cancellationToken);

        // 3. Change Status of the Requests recdord to "accepted", and set ActionedBy and UpdatedDate
        request.ActionedBy = wrapper.Commmand.ActionedBy;
        request.Status = RequestStatus.Accepted;
        request.UpdatedDate = DateTime.UtcNow;

        await _providerRelationshipsDataContext.SaveChangesAsync(cancellationToken);

        return new ValidatedResponse<AcceptCreateAccountRequestCommandResult>();
    }

    private async Task CreateAccountAndAddPermissions(AcceptCreateAccountRequestCommand command, Request request, CancellationToken cancellationToken)
    {
        // 1. Add new records to the Accounts and AccountLegalEntities tables
        // using values from the body (if do not already exist).

        Account account = await GetOrCreateAccount(command, cancellationToken);

        AccountLegalEntity? accountLegalEntity = await GetOrCreateAccountLegalEntity(command, cancellationToken);

        AccountProvider? accountProvider = await GetOrCreateAccountProvider(command, request, cancellationToken);
            
        AccountProviderLegalEntity accountProviderLegalEntity = 
            await _accountProviderLegalEntitiesWriteRepository.CreateAccountProviderLegalEntity(
                command.AccountLegalEntity.Id,
                accountProvider!,
                cancellationToken
        );

        // 3. Then perform the permissions upsert (using the processing as in Set or Update Permissions)
        // with Ukprn from Requests and Operation
        // from the PermissionRequests.

        IEnumerable<Permission> permissions = request.PermissionRequests.Select(pr => new Permission()
        {
            AccountProviderLegalEntityId = command.AccountLegalEntity.Id,
            Operation = (Operation)pr.Operation
        });

        _permissionsWriteRepository.CreatePermissions(permissions);

        await PublishEvent(accountProviderLegalEntity, command, permissions, cancellationToken);

        // 4. The "accountLegalEntity"."id" as AccountLegalEntityId and "actionedBy"
        // as EmployerUserRef plus Ukprn from Requests to
        // be used for the PermissionsAudit for Action = "AccountCreated".
        await CreatePermissionsAudit(
            command,
            request,
            permissions.Select(a => a.Operation), 
            RequestAction.AccountCreated, 
            cancellationToken
        );
        
        await _providerRelationshipsDataContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<Account> GetOrCreateAccount(AcceptCreateAccountRequestCommand command, CancellationToken cancellationToken)
    {
        Account? account;

        try
        {
            account = await _accountWriteRepository.CreateAccount(
                new Account()
                {
                    Id = command.Account.Id,
                    // HashedId = ?, Check EAS - Das.Employer.Apprenticeship service / or Das.Employer.Accounts
                    // PublicHashedId = ?,
                    Name = command.Account.Name,
                    Updated = DateTime.UtcNow,
                    Created = DateTime.UtcNow
                },
                cancellationToken
            );

            await _providerRelationshipsDataContext.SaveChangesAsync(cancellationToken);
        }
        catch(DbUpdateException _exception)
        {
            _logger.LogError(_exception, "Account Id {AccountId} already exists", command.Account.Id);

            account = await _accountReadRepository.GetAccount(command.Account.Id, cancellationToken);
        }

        return account!;
    }

    private async Task<AccountLegalEntity> GetOrCreateAccountLegalEntity(AcceptCreateAccountRequestCommand command, CancellationToken cancellationToken)
    {
        AccountLegalEntity? accountLegalEntity;

        try
        {
            accountLegalEntity = await _accountLegalEntityWriteRepository.CreateAccountLegalEntity(
               new AccountLegalEntity()
               {
                   Id = command.AccountLegalEntity.Id,
                   // PublicHashedId = ?,
                   AccountId = command.Account.Id,
                   Name = command.AccountLegalEntity.Name,
                   Created = DateTime.UtcNow,
                   Updated = DateTime.UtcNow
               },
               cancellationToken
           );

           await _providerRelationshipsDataContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException _exception)
        {
            _logger.LogError(_exception, "Account legal entity Id {AccountLegalEntityId} already exists", command.AccountLegalEntity.Id);

            accountLegalEntity = await _accountLegalEntityReadRepository.GetAccountLegalEntity(command.AccountLegalEntity.Id, cancellationToken);
        }

        return accountLegalEntity!;
    }
    private async Task<AccountProvider> GetOrCreateAccountProvider(AcceptCreateAccountRequestCommand command, Request request, CancellationToken cancellationToken)
    {
        bool hasAddedAccountProvider = false;

        AccountProvider? accountProvider = await _accountProviderWriteRepository.GetAccountProvider(request.Ukprn, command.Account.Id, cancellationToken);

        if (accountProvider is null)
        {
            hasAddedAccountProvider = true;
            accountProvider = await _accountProviderWriteRepository.CreateAccountProvider(
                request.Ukprn,
                command.Account.Id,
                cancellationToken
            );
        }

        if (hasAddedAccountProvider)
        {
            await _messageSession.Publish(
                new AddedAccountProviderEvent(
                    accountProvider.Id, 
                    command.Account.Id, 
                    accountProvider.ProviderUkprn,
                    Guid.Parse(command.ActionedBy), 
                    DateTime.UtcNow, 
                    null
            ), 
            cancellationToken);
        }

        return accountProvider;
    }

    private async Task CreatePermissionsAudit(AcceptCreateAccountRequestCommand command, Request request, IEnumerable<Operation> operations, RequestAction action, CancellationToken cancellationToken)
    {
        PermissionsAudit permissionsAudit = new()
        {
            Eventtime = DateTime.UtcNow,
            Action = action.ToString(),
            Ukprn = request.Ukprn,
            AccountLegalEntityId = command.AccountLegalEntity.Id,
            EmployerUserRef = Guid.Parse(command.ActionedBy),
            Operations = JsonSerializer.Serialize(operations)
        };

        await _permissionsAuditWriteRepository.RecordPermissionsAudit(permissionsAudit, cancellationToken);
    }

    private async Task PublishEvent(
        AccountProviderLegalEntity accountProviderLegalEntity, 
        AcceptCreateAccountRequestCommand command,
        IEnumerable<Permission> permissions,
        CancellationToken cancellationToken
    )
    {
        await _messageSession.Publish(
            new UpdatedPermissionsEvent(
                accountProviderLegalEntity.AccountProvider.AccountId,
                accountProviderLegalEntity.AccountLegalEntityId,
                accountProviderLegalEntity.AccountProvider.Id,
                accountProviderLegalEntity.Id,
                accountProviderLegalEntity.AccountProvider.ProviderUkprn,
                Guid.Parse(command.ActionedBy),
                string.Empty,
                string.Empty,
                string.Empty,
                permissions.Select(a => a.Operation).ToHashSet(),
                [],
                DateTime.UtcNow
            )
        , cancellationToken);
    }
}
