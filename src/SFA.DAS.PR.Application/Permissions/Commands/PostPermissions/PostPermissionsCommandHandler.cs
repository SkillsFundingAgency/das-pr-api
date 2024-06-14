using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Data;
using SFA.DAS.PR.Domain.Common;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using System.Text.Json;

namespace SFA.DAS.PR.Application.Permissions.Commands.PostPermissions;

public class PostPermissionsCommandHandler(
    IAccountProviderLegalEntitiesReadRepository _accountProviderLegalEntitiesReadRepository,
    IAccountLegalEntityReadRepository _accountLegalEntityReadRepository,
    IAccountProviderWriteRepository _accountProviderWriteRepository,
    IAccountProviderLegalEntitiesWriteRepository _accountProviderLegalEntitiesWriteRepository,
    IPermissionsWriteRepository _permissionsWriteRepository,
    IPermissionsAuditWriteRepository _permissionsAuditWriteRepository,
    IProviderRelationshipsDataContext _providerRelationshipsDataContext
) : IRequestHandler<PostPermissionsCommand, ValidatedResponse<PostPermissionsCommandResult>>
{
    public async Task<ValidatedResponse<PostPermissionsCommandResult>> Handle(PostPermissionsCommand command, CancellationToken cancellationToken)
    {
        AccountProviderLegalEntity? accountProviderLegalEntity = await _accountProviderLegalEntitiesReadRepository.GetAccountProviderLegalEntity(
            command.Ukprn,
            command.AccountLegalEntityId,
            cancellationToken
        );

        if (accountProviderLegalEntity != null)
        {
            return await UpdatePermissions(accountProviderLegalEntity, command, cancellationToken);
        }

        AccountLegalEntity? accountLegalEntity = await _accountLegalEntityReadRepository.GetAccountLegalEntity(
            command.AccountLegalEntityId,
            cancellationToken
        );

        if (accountLegalEntity == null)
        {
            return new ValidatedResponse<PostPermissionsCommandResult>(new PostPermissionsCommandResult());
        }

        return await CreateAccountProviderAndAddPermissions(command, accountLegalEntity.AccountId, cancellationToken);
    }

    private async Task<ValidatedResponse<PostPermissionsCommandResult>> CreateAccountProviderAndAddPermissions(PostPermissionsCommand command, long accountId, CancellationToken cancellationToken)
    {
        AccountProvider accountProvider = await _accountProviderWriteRepository.CreateAccountProvider(
            command.Ukprn!.Value,
            accountId,
            cancellationToken
        );

        AccountProviderLegalEntity newAccountProviderLegalEntity = await _accountProviderLegalEntitiesWriteRepository.CreateAccountProviderLegalEntity(
            command.AccountLegalEntityId,
            accountProvider,
            cancellationToken
        );

        IEnumerable<Permission> permissions = command.Operations.Select(operation => new Permission()
            {
                AccountProviderLegalEntity = newAccountProviderLegalEntity,
                Operation = operation
            }
        );

        _permissionsWriteRepository.CreatePermissions(permissions);
        await CreatePermissionsAudit(command, command.Operations, PermissionAuditActions.PermissionCreatedAction, cancellationToken);
        await _providerRelationshipsDataContext.SaveChangesAsync(cancellationToken);

        return new ValidatedResponse<PostPermissionsCommandResult>(new PostPermissionsCommandResult());
    }

    private async Task<ValidatedResponse<PostPermissionsCommandResult>> UpdatePermissions(AccountProviderLegalEntity accountProviderLegalEntity, PostPermissionsCommand command, CancellationToken cancellationToken)
    {
        Operation[] existingOperations = accountProviderLegalEntity.Permissions.Select(po => po.Operation).OrderBy(po => po).ToArray();
        Operation[] commandOperations = command.Operations.OrderBy(co => co).ToArray();

        if (existingOperations.SequenceEqual(commandOperations))
        {
            return new ValidatedResponse<PostPermissionsCommandResult>(new PostPermissionsCommandResult());
        }

        RemovePermissions(accountProviderLegalEntity.Permissions);
        AddPermissions(accountProviderLegalEntity.Id, command.Operations, cancellationToken);
        
        await CreatePermissionsAudit(command, command.Operations, PermissionAuditActions.PermissionUpdatedAction, cancellationToken);
        await _providerRelationshipsDataContext.SaveChangesAsync(cancellationToken);

        return new ValidatedResponse<PostPermissionsCommandResult>(new PostPermissionsCommandResult());
    }

    private void AddPermissions(long accountProviderLegalEntityId, List<Operation> operationsToAdd, CancellationToken cancellationToken)
    {
        if (operationsToAdd.Any())
        {
            Permission[] permissionsToAdd = operationsToAdd.Select(operation => new Permission
            {
                AccountProviderLegalEntityId = accountProviderLegalEntityId,
                Operation = operation
            }).ToArray();

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

    private async Task CreatePermissionsAudit(PostPermissionsCommand command, List<Operation> operations, string action, CancellationToken cancellationToken)
    {
        PermissionsAudit permissionsAudit = new()
        {
            Eventtime = DateTime.UtcNow,
            Action = action,
            Ukprn = command.Ukprn!.Value,
            AccountLegalEntityId = command.AccountLegalEntityId,
            EmployerUserRef = command.UserRef,
            Operations = JsonSerializer.Serialize(operations)
        };

        await _permissionsAuditWriteRepository.RecordPermissionsAudit(permissionsAudit, cancellationToken);
    }
}