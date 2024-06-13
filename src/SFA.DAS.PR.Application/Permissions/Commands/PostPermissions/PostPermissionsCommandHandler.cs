using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Data;
using SFA.DAS.PR.Domain.Common;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using System.Text.Json;

namespace SFA.DAS.PR.Application.Permissions.Commands.PostPermissions;

public class PostPermissionsCommandHandler(
    IAccountProviderLegalEntitiesReadRepository accountProviderLegalEntitiesReadRepository,
    IAccountLegalEntityReadRepository accountLegalEntityReadRepository,
    IAccountProviderWriteRepository accountProviderWriteRepository,
    IAccountProviderLegalEntitiesWriteRepository accountProviderLegalEntitiesWriteRepository,
    IPermissionsWriteRepository permissionsWriteRepository,
    IPermissionsAuditWriteRepository permissionsAuditWriteRepository,
    IProviderRelationshipsDataContext providerRelationshipsDataContext
) : IRequestHandler<PostPermissionsCommand, ValidatedResponse<PostPermissionsCommandResult>>
{
    public async Task<ValidatedResponse<PostPermissionsCommandResult>> Handle(PostPermissionsCommand command, CancellationToken cancellationToken)
    {
        AccountProviderLegalEntity? accountProviderLegalEntity = await accountProviderLegalEntitiesReadRepository.GetAccountProviderLegalEntity(
            command.Ukprn,
            command.AccountLegalEntityId,
            cancellationToken
        );

        if (accountProviderLegalEntity == null)
        {
            AccountLegalEntity? accountLegalEntity = await accountLegalEntityReadRepository.GetAccountLegalEntity(
                command.AccountLegalEntityId,
                cancellationToken
            );

            if (accountLegalEntity == null)
            {
                return new ValidatedResponse<PostPermissionsCommandResult>(new PostPermissionsCommandResult());
            }

            return await CreateAccountProviderAndAddPermissions(command, accountLegalEntity.AccountId, cancellationToken);
        }

        return await CheckExistingPermissions(accountProviderLegalEntity, command, cancellationToken);
    }

    private async Task<ValidatedResponse<PostPermissionsCommandResult>> CreateAccountProviderAndAddPermissions(PostPermissionsCommand command, long accountId, CancellationToken cancellationToken)
    {
        AccountProvider accountProvider = await accountProviderWriteRepository.CreateAccountProvider(
            command.Ukprn!.Value,
            accountId,
            cancellationToken
        );

        AccountProviderLegalEntity newAccountProviderLegalEntity = await accountProviderLegalEntitiesWriteRepository.CreateAccountProviderLegalEntity(
            command.AccountLegalEntityId,
            accountProvider,
            cancellationToken
        );

        Permission[] permissions = command.Operations.Select(operation => new Permission()
            {
                AccountProviderLegalEntity = newAccountProviderLegalEntity,
                Operation = operation
            }
        ).ToArray();

        await permissionsWriteRepository.CreatePermissions(permissions, cancellationToken);
        await CreatePermissionsAudit(command, command.Operations, PermissionAuditActions.PermissionCreatedAction, cancellationToken);
        await providerRelationshipsDataContext.SaveChangesAsync(cancellationToken);

        return new ValidatedResponse<PostPermissionsCommandResult>(new PostPermissionsCommandResult());
    }

    private async Task<ValidatedResponse<PostPermissionsCommandResult>> CheckExistingPermissions(AccountProviderLegalEntity accountProviderLegalEntity, PostPermissionsCommand command, CancellationToken cancellationToken)
    {
        Operation[] existingOperations = accountProviderLegalEntity.Permissions.Select(po => po.Operation).OrderBy(po => po).ToArray();
        Operation[] commandOperations = command.Operations.OrderBy(co => co).ToArray();

        if (existingOperations.SequenceEqual(commandOperations))
        {
            return new ValidatedResponse<PostPermissionsCommandResult>(new PostPermissionsCommandResult());
        }

        RemovePermissions(accountProviderLegalEntity.Permissions);
        await AddPermissions(accountProviderLegalEntity.Id, command.Operations, cancellationToken);
        
        await CreatePermissionsAudit(command, command.Operations, PermissionAuditActions.PermissionUpdatedAction, cancellationToken);
        await providerRelationshipsDataContext.SaveChangesAsync(cancellationToken);

        return new ValidatedResponse<PostPermissionsCommandResult>(new PostPermissionsCommandResult());
    }

    private async Task AddPermissions(long accountProviderLegalEntityId, List<Operation> operationsToAdd, CancellationToken cancellationToken)
    {
        if (operationsToAdd.Any())
        {
            Permission[] permissionsToAdd = operationsToAdd.Select(operation => new Permission
            {
                AccountProviderLegalEntityId = accountProviderLegalEntityId,
                Operation = operation
            }).ToArray();

            await permissionsWriteRepository.CreatePermissions(permissionsToAdd, cancellationToken);
        }
    }

    private void RemovePermissions(IEnumerable<Permission> existingPermissions)
    {
        if (existingPermissions.Any())
        {
            Permission[] permissionsToDelete = existingPermissions.ToArray();
            permissionsWriteRepository.DeletePermissions(permissionsToDelete);
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

        await permissionsAuditWriteRepository.RecordPermissionsAudit(permissionsAudit, cancellationToken);
    }
}