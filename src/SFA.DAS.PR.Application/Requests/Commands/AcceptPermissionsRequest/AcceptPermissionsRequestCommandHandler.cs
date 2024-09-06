using System.Text.Json;
using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Data;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.PR.Application.Requests.Commands.AcceptPermissionsRequest;

public sealed class AcceptPermissionsRequestCommandHandler(
    IProviderRelationshipsDataContext _providerRelationshipsDataContext,
    IRequestWriteRepository _requestWriteRepository,
    IAccountProviderLegalEntitiesReadRepository _accountProviderLegalEntitiesReadRepository,
    IAccountLegalEntityReadRepository _accountLegalEntityReadRepository,
    IPermissionsWriteRepository _permissionsWriteRepository,
    IMessageSession _messageSession,
    IPermissionsAuditWriteRepository _permissionsAuditWriteRepository
) : IRequestHandler<AcceptPermissionsRequestCommand, ValidatedResponse<Unit>>
{
    public async Task<ValidatedResponse<Unit>> Handle(AcceptPermissionsRequestCommand command, CancellationToken cancellationToken)
    {
        Request request = (await _requestWriteRepository.GetRequest(command.RequestId, cancellationToken))!;

        AcceptRequest(request, command.ActionedBy);

        AccountLegalEntity accountLegalEntity = (await _accountLegalEntityReadRepository.GetAccountLegalEntity(
            request.AccountLegalEntityId!.Value, 
            cancellationToken
        ))!;

        AccountProviderLegalEntity? accountProviderLegalEntity = await _accountProviderLegalEntitiesReadRepository.GetAccountProviderLegalEntity(
            request.Ukprn,
            accountLegalEntity.Id,
            cancellationToken
        );

        if (accountProviderLegalEntity is not null)
        {
            Operation[] operations = request.PermissionRequests.Select(pr =>(Operation)pr.Operation).ToArray();

            bool permissionsUpdated = UpdatePermissions(operations, accountProviderLegalEntity);

            if(permissionsUpdated)
            {
                await CreatePermissionUpdatedAudit(command, request, operations, cancellationToken);

                await PublishUpdatedPermissionsEvent(accountProviderLegalEntity, command, operations, cancellationToken);
            }
        }

        await _providerRelationshipsDataContext.SaveChangesAsync(cancellationToken);

        return ValidatedResponse<Unit>.EmptySuccessResponse();
    }

    private bool UpdatePermissions(Operation[] requestOperations, AccountProviderLegalEntity accountProviderLegalEntity)
    {
        Operation[] existingOperations = accountProviderLegalEntity.Permissions.Select(p => p.Operation).OrderBy(o => o).ToArray();

        if (existingOperations.SequenceEqual(requestOperations))
        {
            return false;
        }

        RemovePermissions(accountProviderLegalEntity.Permissions);

        AddPermissions(accountProviderLegalEntity.Id, requestOperations);

        return true;
    }

    private void AddPermissions(long accountProviderLegalEntityId, Operation[] operationsToAdd)
    {
        if (operationsToAdd.Any())
        {
            IEnumerable<Permission> permissionsToAdd = operationsToAdd.Select(operation => new Permission
            {
                AccountProviderLegalEntityId = accountProviderLegalEntityId,
                Operation = operation
            });

            _permissionsWriteRepository.CreatePermissions(permissionsToAdd);
        }
    }

    private void RemovePermissions(IEnumerable<Permission> existingPermissions)
    {
        if (existingPermissions.Any())
        {
            _permissionsWriteRepository.DeletePermissions(existingPermissions);
        }
    }

    private static void AcceptRequest(Request request, string actionedBy)
    {
        request.Status = RequestStatus.Accepted;
        request.ActionedBy = actionedBy;
        request.UpdatedDate = DateTime.UtcNow;
    }

    private async Task CreatePermissionUpdatedAudit(AcceptPermissionsRequestCommand command, Request request, IEnumerable<Operation> operations, CancellationToken cancellationToken)
    {
        PermissionsAudit permissionsAudit = new()
        {
            Eventtime = DateTime.UtcNow,
            Action = nameof(PermissionAction.PermissionUpdated),
            Ukprn = request.Ukprn,
            AccountLegalEntityId = request.AccountLegalEntityId!.Value,
            EmployerUserRef = Guid.Parse(command.ActionedBy),
            Operations = JsonSerializer.Serialize(operations)
        };

        await _permissionsAuditWriteRepository.RecordPermissionsAudit(permissionsAudit, cancellationToken);
    }

    private async Task PublishUpdatedPermissionsEvent(
        AccountProviderLegalEntity accountProviderLegalEntity,
        AcceptPermissionsRequestCommand command,
        Operation[] operations,
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
                operations.ToHashSet(),
                [],
                DateTime.UtcNow
            )
        , cancellationToken);
    }
}
