using System.Text.Json;
using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Data;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.PR.Application.Requests.Commands.AcceptAddAccountRequest;

public sealed class AcceptAddAccountRequestCommandHandler(
    IProviderRelationshipsDataContext _providerRelationshipsDataContext,
    IRequestWriteRepository _requestWriteRepository,
    IAccountProviderLegalEntitiesWriteRepository _accountProviderLegalEntitiesWriteRepository,
    IAccountProviderWriteRepository _accountProviderWriteRepository,
    IAccountLegalEntityReadRepository _accountLegalEntityReadRepository,
    IPermissionsWriteRepository _permissionsWriteRepository,
    IMessageSession _messageSession,
    IPermissionsAuditWriteRepository _permissionsAuditWriteRepository
) : IRequestHandler<AcceptAddAccountRequestCommand, ValidatedResponse<Unit>>
{
    public async Task<ValidatedResponse<Unit>> Handle(AcceptAddAccountRequestCommand command, CancellationToken cancellationToken)
    {
        Request request = (await _requestWriteRepository.GetRequest(command.RequestId, cancellationToken))!;

        AcceptRequest(request, command.ActionedBy);

        AccountLegalEntity accountLegalEntity = (await _accountLegalEntityReadRepository.GetAccountLegalEntity(
            request.AccountLegalEntityId!.Value,
            cancellationToken
        ))!;

        AccountProvider accountProvider = (await _accountProviderWriteRepository.GetAccountProvider(
            request.Ukprn,
            accountLegalEntity.AccountId,
            cancellationToken
        ))!;

        AccountProviderLegalEntity accountProviderLegalEntity =
            await _accountProviderLegalEntitiesWriteRepository.CreateAccountProviderLegalEntity(
                request.AccountLegalEntityId!.Value,
                accountProvider,
                cancellationToken
        );

        IEnumerable<Permission> permissions = request.PermissionRequests.Select(pr => new Permission()
        {
            AccountProviderLegalEntity = accountProviderLegalEntity,
            Operation = (Operation)pr.Operation
        });

        _permissionsWriteRepository.CreatePermissions(permissions);

        await CreatePermissionUpdatedAudit(command, request, permissions.Select(a => a.Operation), cancellationToken);

        await PublishUpdatedPermissionsEvent(accountProviderLegalEntity, command, permissions, cancellationToken);

        await _providerRelationshipsDataContext.SaveChangesAsync(cancellationToken);

        return ValidatedResponse<Unit>.EmptySuccessResponse();
    }

    private static void AcceptRequest(Request request, string actionedBy)
    {
        request.Status = RequestStatus.Accepted;
        request.ActionedBy = actionedBy;
        request.UpdatedDate = DateTime.UtcNow;
    }

    private async Task CreatePermissionUpdatedAudit(AcceptAddAccountRequestCommand command, Request request, IEnumerable<Operation> operations, CancellationToken cancellationToken)
    {
        PermissionsAudit permissionsAudit = new()
        {
            Eventtime = DateTime.UtcNow,
            Action = nameof(RequestAction.AccountAdded),
            Ukprn = request.Ukprn,
            AccountLegalEntityId = request.AccountLegalEntityId!.Value,
            EmployerUserRef = Guid.Parse(command.ActionedBy),
            Operations = JsonSerializer.Serialize(operations)
        };

        await _permissionsAuditWriteRepository.RecordPermissionsAudit(
            permissionsAudit, 
            cancellationToken
        );
    }

    private async Task PublishUpdatedPermissionsEvent(
        AccountProviderLegalEntity accountProviderLegalEntity,
        AcceptAddAccountRequestCommand command,
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
            ), 
            cancellationToken
        );
    }
}
