using System.Text.Json;
using MediatR;
using SFA.DAS.PR.Application.Common.Commands;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Data;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.PR.Application.Permissions.Commands.RemovePermissions;

public class RemovePermissionsCommandHandler(
    IAccountProviderLegalEntitiesReadRepository _accountProviderLegalEntitiesReadRepository,
    IPermissionsWriteRepository _permissionsWriteRepository,
    IPermissionsAuditWriteRepository _permissionsAuditWriteRepository,
    IProviderRelationshipsDataContext _providerRelationshipsDataContext,
    IMessageSession _messageSession
) : IRequestHandler<RemovePermissionsCommand, ValidatedResponse<SuccessCommandResult>>
{
    public async Task<ValidatedResponse<SuccessCommandResult>> Handle(RemovePermissionsCommand command, CancellationToken cancellationToken)
    {
        AccountProviderLegalEntity? accountProviderLegalEntity = await _accountProviderLegalEntitiesReadRepository.GetAccountProviderLegalEntity(
            command.Ukprn,
            command.AccountLegalEntityId,
            cancellationToken
        );

        if (accountProviderLegalEntity != null)
        {
            HashSet<Operation> existingPermissions = accountProviderLegalEntity.Permissions.Select(po => po.Operation).OrderBy(po => po).ToHashSet();

            await RemoveAndAuditPermissions(accountProviderLegalEntity, command, cancellationToken);

            await PublishEvent(accountProviderLegalEntity, command.UserRef, existingPermissions, cancellationToken);
        }

        return new ValidatedResponse<SuccessCommandResult>();
    }

    private async Task RemoveAndAuditPermissions(AccountProviderLegalEntity accountProviderLegalEntity, RemovePermissionsCommand command, CancellationToken cancellationToken)
    {
        var operationsToRemove = accountProviderLegalEntity.Permissions.Select(permission => permission.Operation);

        RemovePermissions(accountProviderLegalEntity.Permissions);

        await DeletePermissionsAudit(command, PermissionAction.PermissionDeleted.ToString(), operationsToRemove, cancellationToken);
        await _providerRelationshipsDataContext.SaveChangesAsync(cancellationToken);

    }

    private void RemovePermissions(List<Permission> permissions)
    {
        if (permissions is { Count: > 0 })
        {
            _permissionsWriteRepository.DeletePermissions(permissions);
        }
    }

    private async Task DeletePermissionsAudit(RemovePermissionsCommand command, string action, IEnumerable<Operation> operationsToRemove, CancellationToken cancellationToken)
    {
        PermissionsAudit permissionsAudit = new PermissionsAudit
        {
            Eventtime = DateTime.UtcNow,
            Action = action,
            Ukprn = command.Ukprn!.Value,
            AccountLegalEntityId = command.AccountLegalEntityId,
            EmployerUserRef = command.UserRef,
            Operations = JsonSerializer.Serialize(operationsToRemove)
        };

        await _permissionsAuditWriteRepository.RecordPermissionsAudit(permissionsAudit, cancellationToken);
    }

    private async Task PublishEvent(AccountProviderLegalEntity accountProviderLegalEntity, Guid userRef, HashSet<Operation> existingPermissions, CancellationToken cancellationToken)
    {
        await _messageSession.Publish(
            new UpdatedPermissionsEvent(
                accountProviderLegalEntity.AccountProvider.AccountId,
                accountProviderLegalEntity.AccountLegalEntityId,
                accountProviderLegalEntity.AccountProvider.Id,
                accountProviderLegalEntity.Id,
                accountProviderLegalEntity.AccountProvider.ProviderUkprn,
                userRef,
                string.Empty,
                string.Empty,
                string.Empty,
                [],
                existingPermissions,
                DateTime.UtcNow)
            , cancellationToken);
    }
}
