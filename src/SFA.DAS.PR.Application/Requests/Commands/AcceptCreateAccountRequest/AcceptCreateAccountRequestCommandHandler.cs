using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SFA.DAS.Encoding;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Data;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.PR.Application.Requests.Commands.AcceptCreateAccountRequest;

public sealed class AcceptCreateAccountRequestCommandHandler(
    ILogger<AcceptCreateAccountRequestCommandHandler> _logger,
    IEncodingService _encodingService,
    IProviderRelationshipsDataContext _providerRelationshipsDataContext,
    IAccountWriteRepository _accountWriteRepository,
    IAccountReadRepository _accountReadRepository,
    IAccountProviderWriteRepository _accountProviderWriteRepository,
    IAccountLegalEntityReadRepository _accountLegalEntityReadRepository,
    IAccountLegalEntityWriteRepository _accountLegalEntityWriteRepository,
    IAccountProviderLegalEntitiesWriteRepository _accountProviderLegalEntitiesWriteRepository,
    IPermissionsWriteRepository _permissionsWriteRepository,
    IPermissionsAuditWriteRepository _permissionsAuditWriteRepository,
    IMessageSession _messageSession,
    IRequestReadRepository requestReadRepository
) : IRequestHandler<AcceptCreateAccountRequestCommand, ValidatedResponse<Unit>>
{
    public async Task<ValidatedResponse<Unit>> Handle(AcceptCreateAccountRequestCommand command, CancellationToken cancellationToken)
    {
        await CreateAccountAndAddPermissions(command, cancellationToken);

        return ValidatedResponse<Unit>.EmptySuccessResponse();
    }

    private async Task CreateAccountAndAddPermissions(AcceptCreateAccountRequestCommand command, CancellationToken cancellationToken)
    {
        Request? request = await requestReadRepository.GetRequest(command.RequestId, cancellationToken);

        Account account = await GetOrCreateAccount(command, cancellationToken);

        AccountLegalEntity? accountLegalEntity = await GetOrCreateAccountLegalEntity(command, cancellationToken);

        Tuple<bool, AccountProvider> providerResponse = await GetOrCreateAccountProvider(command, request!, cancellationToken);
            
        AccountProviderLegalEntity accountProviderLegalEntity = 
            await _accountProviderLegalEntitiesWriteRepository.CreateAccountProviderLegalEntity(
                command.AccountLegalEntity.Id,
                providerResponse.Item2,
                cancellationToken
        );

        IEnumerable<Permission> permissions = request!.PermissionRequests.Select(pr => new Permission()
        {
            AccountProviderLegalEntity = accountProviderLegalEntity,
            Operation = (Operation)pr.Operation
        });

        _permissionsWriteRepository.CreatePermissions(permissions);

        await CreatePermissionsAudit(
            command,
            request,
            permissions.Select(a => a.Operation), 
            RequestAction.AccountCreated, 
            cancellationToken
        );

        request.ActionedBy = command.ActionedBy;
        request.Status = RequestStatus.Accepted;
        request.UpdatedDate = DateTime.UtcNow;

        await _providerRelationshipsDataContext.SaveChangesAsync(cancellationToken);

        if (providerResponse.Item1)
        {
            await _messageSession.Publish(
                new AddedAccountProviderEvent(
                    providerResponse.Item2.Id,
                    command.Account.Id,
                    providerResponse.Item2.ProviderUkprn,
                    Guid.Parse(command.ActionedBy),
                    DateTime.UtcNow,
                    null
            ),
            cancellationToken);
        }

        await PublishEvent(accountProviderLegalEntity, command, permissions, cancellationToken);
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
                    HashedId = _encodingService.Encode(command.Account.Id, EncodingType.AccountId),
                    PublicHashedId = _encodingService.Encode(command.Account.Id, EncodingType.PublicAccountId),
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
                   PublicHashedId = _encodingService.Encode(command.AccountLegalEntity.Id, EncodingType.PublicAccountLegalEntityId),
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
    private async Task<Tuple<bool, AccountProvider>> GetOrCreateAccountProvider(AcceptCreateAccountRequestCommand command, Request request, CancellationToken cancellationToken)
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

        return new (hasAddedAccountProvider, accountProvider);
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
