using System.Text.Json;
using System.Threading;
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

record struct ProviderResponse(bool ProviderAdded, AccountProvider? AcountProvider);

public sealed class AcceptCreateAccountRequestCommandHandler(
    ILogger<AcceptCreateAccountRequestCommandHandler> _logger,
    IEncodingService _encodingService,
    IProviderRelationshipsDataContext _providerRelationshipsDataContext,
    IAccountWriteRepository _accountWriteRepository,
    IAccountProviderWriteRepository _accountProviderWriteRepository,
    IAccountLegalEntityWriteRepository _accountLegalEntityWriteRepository,
    IAccountProviderLegalEntitiesWriteRepository _accountProviderLegalEntitiesWriteRepository,
    IPermissionsWriteRepository _permissionsWriteRepository,
    IPermissionsAuditWriteRepository _permissionsAuditWriteRepository,
    IMessageSession _messageSession,
    IRequestWriteRepository _requestWriteRepository
) : IRequestHandler<AcceptCreateAccountRequestCommand, ValidatedResponse<Unit>>
{
    public async Task<ValidatedResponse<Unit>> Handle(AcceptCreateAccountRequestCommand command, CancellationToken cancellationToken)
    {
        await CreateAccountAndAddPermissions(command, cancellationToken);

        return ValidatedResponse<Unit>.EmptySuccessResponse();
    }

    private async Task CreateAccountAndAddPermissions(AcceptCreateAccountRequestCommand command, CancellationToken cancellationToken)
    {
        Request request = (await _requestWriteRepository.GetRequest(command.RequestId, cancellationToken))!;

        await CreateAccount(command, cancellationToken);

        await CreateAccountLegalEntity(command, cancellationToken);

        ProviderResponse providerResponse = await GetOrCreateAccountProvider(command, request, cancellationToken);
            
        AccountProviderLegalEntity accountProviderLegalEntity = 
            await _accountProviderLegalEntitiesWriteRepository.CreateAccountProviderLegalEntity(
                command.AccountLegalEntity.Id,
                providerResponse.AcountProvider!,
                cancellationToken
        );

        IEnumerable<Permission> permissions = request.PermissionRequests.Select(pr => new Permission()
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

        AcceptRequest(request, command);

        if (providerResponse.ProviderAdded)
        {
            await PublishAddedAccountProviderEvent(command, providerResponse, cancellationToken);
        }

        await PublishUpdatedPermissionsEvent(accountProviderLegalEntity, command, permissions, cancellationToken);

        await _providerRelationshipsDataContext.SaveChangesAsync(cancellationToken);
    }

    private static void AcceptRequest(Request request, AcceptCreateAccountRequestCommand command)
    {
        request.ActionedBy = command.ActionedBy;
        request.Status = RequestStatus.Accepted;
        request.UpdatedDate = DateTime.UtcNow;
    }

    private async Task CreateAccount(AcceptCreateAccountRequestCommand command, CancellationToken cancellationToken)
    {
        try
        {
            await _accountWriteRepository.CreateAccount(
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
        catch (DbUpdateException _exception)
        {
            _logger.LogError(_exception, "Account Id {AccountId} already exists", command.Account.Id);
        }
    }

    private async Task CreateAccountLegalEntity(AcceptCreateAccountRequestCommand command, CancellationToken cancellationToken)
    {
        try
        {
            await _accountLegalEntityWriteRepository.CreateAccountLegalEntity(
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
        }
    }
    private async Task<ProviderResponse> GetOrCreateAccountProvider(AcceptCreateAccountRequestCommand command, Request request, CancellationToken cancellationToken)
    {
        var accountProvider = await _accountProviderWriteRepository.GetAccountProvider(request.Ukprn, command.Account.Id, cancellationToken);

        ProviderResponse providerResponse = new ProviderResponse(false, accountProvider);

        if (providerResponse.AcountProvider is null)
        {
            providerResponse.ProviderAdded = true;
            providerResponse.AcountProvider = await _accountProviderWriteRepository.CreateAccountProvider(
                request.Ukprn,
                command.Account.Id,
                cancellationToken
            );
        }

        return providerResponse;
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

    private async Task PublishAddedAccountProviderEvent(AcceptCreateAccountRequestCommand command, ProviderResponse providerResponse, CancellationToken cancellationToken)
    {
        await _messageSession.Publish(
            new AddedAccountProviderEvent(
                providerResponse.AcountProvider!.Id,
                command.Account.Id,
                providerResponse.AcountProvider.ProviderUkprn,
                Guid.Parse(command.ActionedBy),
                DateTime.UtcNow,
                null
        ),
        cancellationToken);
    }

    private async Task PublishUpdatedPermissionsEvent(
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
