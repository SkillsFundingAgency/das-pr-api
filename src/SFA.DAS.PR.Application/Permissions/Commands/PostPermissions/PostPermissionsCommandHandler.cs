using System.Text.Json;
using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Data;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.PR.Application.Permissions.Commands.PostPermissions;

public class PostPermissionsCommandHandler(
    IAccountProviderLegalEntitiesReadRepository _accountProviderLegalEntitiesReadRepository,
    IAccountLegalEntityReadRepository _accountLegalEntityReadRepository,
    IAccountProviderWriteRepository _accountProviderWriteRepository,
    IAccountProviderLegalEntitiesWriteRepository _accountProviderLegalEntitiesWriteRepository,
    IPermissionsWriteRepository _permissionsWriteRepository,
    IPermissionsAuditWriteRepository _permissionsAuditWriteRepository,
    IProviderRelationshipsDataContext _providerRelationshipsDataContext,
    IMessageSession _messageSession)
    : IRequestHandler<PostPermissionsCommand, ValidatedResponse<PostPermissionsCommandResult>>
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

        /// validator has already checked the existance of AccountLegalEntity
        AccountLegalEntity? accountLegalEntity = await _accountLegalEntityReadRepository.GetAccountLegalEntity(command.AccountLegalEntityId, cancellationToken);

        return await CreateAccountProviderAndAddPermissions(command, accountLegalEntity!.AccountId, cancellationToken);
    }

    private async Task<ValidatedResponse<PostPermissionsCommandResult>> CreateAccountProviderAndAddPermissions(PostPermissionsCommand command, long accountId, CancellationToken cancellationToken)
    {
        var hasAddedAccountProvider = false;
        AccountProvider? accountProvider = await _accountProviderWriteRepository.GetAccountProvider(command.Ukprn, accountId, cancellationToken);
        if (accountProvider == null)
        {
            hasAddedAccountProvider = true;
            accountProvider = await _accountProviderWriteRepository.CreateAccountProvider(
                command.Ukprn!.Value,
                accountId,
                cancellationToken
            );
        }

        AccountProviderLegalEntity accountProviderLegalEntity = await _accountProviderLegalEntitiesWriteRepository.CreateAccountProviderLegalEntity(
            command.AccountLegalEntityId,
            accountProvider,
            cancellationToken
        );

        IEnumerable<Permission> permissions = command.Operations.Select(operation => new Permission()
        {
            AccountProviderLegalEntity = accountProviderLegalEntity,
            Operation = operation
        });

        _permissionsWriteRepository.CreatePermissions(permissions);
        await CreatePermissionsAudit(command, command.Operations, PermissionAction.PermissionCreated, cancellationToken);

        await _providerRelationshipsDataContext.SaveChangesAsync(cancellationToken);

        if (hasAddedAccountProvider)
        {
            await _messageSession.Publish(new AddedAccountProviderEvent(accountProvider.Id, accountId, accountProvider.ProviderUkprn, command.UserRef, DateTime.UtcNow, null), cancellationToken);
        }

        await PublishEvent(accountProviderLegalEntity, command, Enumerable.Empty<Operation>(), cancellationToken);

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
        AddPermissions(accountProviderLegalEntity.Id, command.Operations);

        await CreatePermissionsAudit(command, command.Operations, PermissionAction.PermissionUpdated, cancellationToken);
        await _providerRelationshipsDataContext.SaveChangesAsync(cancellationToken);

        await PublishEvent(accountProviderLegalEntity, command, existingOperations, cancellationToken);

        return new ValidatedResponse<PostPermissionsCommandResult>(new PostPermissionsCommandResult());
    }

    private async Task PublishEvent(AccountProviderLegalEntity accountProviderLegalEntity, PostPermissionsCommand command, IEnumerable<Operation> existingPermissions, CancellationToken cancellationToken)
    {
        await _messageSession.Publish(
            new UpdatedPermissionsEvent(
                accountProviderLegalEntity.AccountProvider.AccountId,
                accountProviderLegalEntity.AccountLegalEntityId,
                accountProviderLegalEntity.AccountProvider.Id,
                accountProviderLegalEntity.Id,
                accountProviderLegalEntity.AccountProvider.ProviderUkprn,
                command.UserRef,
                string.Empty,
                string.Empty,
                string.Empty,
                command.Operations.ToHashSet(),
                existingPermissions.ToHashSet(),
                DateTime.UtcNow)
            , cancellationToken);
    }

    private void AddPermissions(long accountProviderLegalEntityId, List<Operation> operationsToAdd)
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

    private async Task CreatePermissionsAudit(PostPermissionsCommand command, List<Operation> operations, PermissionAction action, CancellationToken cancellationToken)
    {
        PermissionsAudit permissionsAudit = new()
        {
            Eventtime = DateTime.UtcNow,
            Action = action.ToString(),
            Ukprn = command.Ukprn!.Value,
            AccountLegalEntityId = command.AccountLegalEntityId,
            EmployerUserRef = command.UserRef,
            Operations = JsonSerializer.Serialize(operations)
        };

        await _permissionsAuditWriteRepository.RecordPermissionsAudit(permissionsAudit, cancellationToken);
    }
}