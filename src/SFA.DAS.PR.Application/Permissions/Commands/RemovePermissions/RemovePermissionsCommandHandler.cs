using MediatR;
using SFA.DAS.PR.Application.Common.Commands;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Data;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.ProviderRelationships.Types.Models;
using System.Text.Json;

namespace SFA.DAS.PR.Application.Permissions.Commands.RemovePermissions;

public class RemovePermissionsCommandHandler(
    IAccountProviderLegalEntitiesReadRepository _accountProviderLegalEntitiesReadRepository,
    IPermissionsWriteRepository _permissionsWriteRepository,
    IPermissionsAuditWriteRepository _permissionsAuditWriteRepository,
    IProviderRelationshipsDataContext _providerRelationshipsDataContext
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
            return await RemoveAndAuditPermissions(accountProviderLegalEntity, command, cancellationToken);
        }

        return new ValidatedResponse<SuccessCommandResult>();
    }

    private async Task<ValidatedResponse<SuccessCommandResult>> RemoveAndAuditPermissions(AccountProviderLegalEntity accountProviderLegalEntity, RemovePermissionsCommand command, CancellationToken cancellationToken)
    {
        var operationsToRemove = accountProviderLegalEntity.Permissions.Select(permission => permission.Operation);

        RemovePermissions(accountProviderLegalEntity.Permissions);

        await DeletePermissionsAudit(command, PermissionAction.PermissionDeleted.ToString(), operationsToRemove, cancellationToken);
        await _providerRelationshipsDataContext.SaveChangesAsync(cancellationToken);

        return new ValidatedResponse<SuccessCommandResult>();
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
}