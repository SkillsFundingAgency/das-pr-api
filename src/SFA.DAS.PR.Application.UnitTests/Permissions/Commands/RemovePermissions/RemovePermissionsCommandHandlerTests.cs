using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using SFA.DAS.PR.Application.Common.Commands;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Application.Permissions.Commands.RemovePermissions;
using SFA.DAS.PR.Data;
using SFA.DAS.PR.Data.Repositories;
using SFA.DAS.PR.Data.UnitTests.InMemoryDatabases;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Text.Json;

namespace SFA.DAS.PR.Application.UnitTests.Permissions.Commands.RemovePermissions;
public class RemovePermissionsCommandHandlerTests
{
    private readonly CancellationToken _cancellationToken = CancellationToken.None;

    [Test, RecursiveMoqAutoData]
    public async Task Handle_RemovePermissions_Null_Entities_Returns_Null(
        [Frozen] Mock<IAccountProviderLegalEntitiesReadRepository> accountProviderLegalEntitiesReadRepository,
        [Frozen] Mock<IAccountLegalEntityReadRepository> accountLegalEntityReadRepository,
        RemovePermissionsCommandHandler sut,
        RemovePermissionsCommand command,
        CancellationToken cancelToken
    )
    {
        accountProviderLegalEntitiesReadRepository.Setup(a =>
            a.GetAccountProviderLegalEntity(
                command.Ukprn,
                command.AccountLegalEntityId,
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync((AccountProviderLegalEntity?)null);

        accountLegalEntityReadRepository.Setup(a =>
            a.GetAccountLegalEntity(command.AccountLegalEntityId, cancelToken)
        ).ReturnsAsync((AccountLegalEntity?)null);

        ValidatedResponse<SuccessCommandResult> result = await sut.Handle(command, cancelToken);
        result.Result.Should().BeNull();
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_RemovePermissions_Remove_Permissions_Adds_Audit_Trail(
        [Frozen] Mock<IAccountProviderLegalEntitiesReadRepository> accountProviderLegalEntitiesReadRepository,
        AccountProviderLegalEntity accountProviderLegalEntity,
        RemovePermissionsCommand command
    )
    {
        accountProviderLegalEntitiesReadRepository.Setup(a =>
            a.GetAccountProviderLegalEntity(
                command.Ukprn,
                command.AccountLegalEntityId,
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(accountProviderLegalEntity);

        ValidatedResponse<SuccessCommandResult> result = null!;


        PermissionsAudit? audit = null;

        var permissionsBeforeRemovalCount = 0;
        var permissionsAfterRemovalCount = -1;
        var operationsRemoved = new List<Operation>();
        var operationsRemovedAsValue = string.Empty;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(Handle_RemovePermissions_Remove_Permissions_Adds_Audit_Trail)}")
        )
        {
            await context.AccountProviderLegalEntities.AddAsync(accountProviderLegalEntity, _cancellationToken);
            await context.SaveChangesAsync(_cancellationToken);

            RemovePermissionsCommandHandler sut = CreateRemovePermissionsCommandHandler(
               accountProviderLegalEntitiesReadRepository.Object,
               context
            );
            permissionsBeforeRemovalCount = await context.Permissions.CountAsync(a => a.AccountProviderLegalEntityId == accountProviderLegalEntity.Id, cancellationToken: _cancellationToken);

            operationsRemoved = accountProviderLegalEntity.Permissions.Select(permission => permission.Operation).ToList();

            operationsRemovedAsValue = JsonSerializer.Serialize(operationsRemoved);

            result = await sut.Handle(command, _cancellationToken);

            permissionsAfterRemovalCount = await context.Permissions.CountAsync(a => a.AccountProviderLegalEntityId == accountProviderLegalEntity.Id, cancellationToken: _cancellationToken);
            audit = await context.PermissionsAudit.FirstOrDefaultAsync(a => a.AccountLegalEntityId == command.AccountLegalEntityId && a.Ukprn == command.Ukprn!.Value, cancellationToken: _cancellationToken);
        }

        result.Result.Should().BeNull();

        Assert.Multiple(() =>
        {
            Assert.That(audit, Is.Not.Null, "Audit must have been recorded.");
            Assert.That(audit?.Action, Is.EqualTo(PermissionAction.PermissionDeleted.ToString()), "Audit action must equal PermissionDeleted.");
            Assert.That(audit?.Operations, Is.EqualTo(operationsRemovedAsValue), "Operations removed audit does not match operations removed");
            Assert.That(permissionsBeforeRemovalCount, Is.GreaterThan(0), "Permissions before removal should be greater than 0");
            Assert.That(permissionsAfterRemovalCount, Is.EqualTo(0), "Permissions after removal should be 0");
        });
    }

    private static RemovePermissionsCommandHandler CreateRemovePermissionsCommandHandler(
        IAccountProviderLegalEntitiesReadRepository accountProviderLegalEntitiesReadRepository,
        ProviderRelationshipsDataContext context
    )
    {
        PermissionsWriteRepository permissionsWriteRepository = new(context);
        PermissionsAuditWriteRepository permissionsAuditWriteRepository = new(context);

        return new RemovePermissionsCommandHandler(
            accountProviderLegalEntitiesReadRepository,
            permissionsWriteRepository,
            permissionsAuditWriteRepository, context);
    }
}
