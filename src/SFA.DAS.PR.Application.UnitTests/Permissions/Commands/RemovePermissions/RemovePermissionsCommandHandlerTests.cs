using System.Text.Json;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
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
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Application.UnitTests.Permissions.Commands.RemovePermissions;
public class RemovePermissionsCommandHandlerTests
{
    private readonly CancellationToken _cancellationToken = CancellationToken.None;

    [Test, RecursiveMoqAutoData]
    public async Task Handle_PermissionsDoNotExist_ReturnsWithoutEffect(
        [Frozen] Mock<IAccountProviderLegalEntitiesReadRepository> accountProviderLegalEntitiesReadRepositoryMock,
        [Frozen] Mock<IPermissionsWriteRepository> _permissionsWriteRepositoryMock,
        [Frozen] Mock<IMessageSession> _messageSessionMock,
        RemovePermissionsCommandHandler sut,
        RemovePermissionsCommand command,
        CancellationToken cancelToken
    )
    {
        accountProviderLegalEntitiesReadRepositoryMock.Setup(a =>
            a.GetAccountProviderLegalEntity(
                command.Ukprn,
                command.AccountLegalEntityId,
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync((AccountProviderLegalEntity?)null);

        /// Act
        await sut.Handle(command, cancelToken);

        _permissionsWriteRepositoryMock.Verify(r => r.DeletePermissions(It.IsAny<IEnumerable<Permission>>()), Times.Never);
        _messageSessionMock.Verify(m => m.Publish(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_PermissionsExists_RemovesPermissions(
        AccountProviderLegalEntity accountProviderLegalEntity,
        RemovePermissionsCommand command
    )
    {
        Mock<IAccountProviderLegalEntitiesReadRepository> accountProviderLegalEntitiesReadRepositoryMock = new();
        accountProviderLegalEntitiesReadRepositoryMock.Setup(a =>
            a.GetAccountProviderLegalEntity(
                command.Ukprn,
                command.AccountLegalEntityId,
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(accountProviderLegalEntity);

        Mock<IMessageSession> _messageSessionMock = new();


        ValidatedResponse<SuccessCommandResult> result = null!;

        PermissionsAudit? audit = null;

        var permissionsBeforeRemovalCount = 0;
        var permissionsAfterRemovalCount = -1;
        var operationsRemovedAsValue = string.Empty;

        var operationsRemoved = accountProviderLegalEntity.Permissions.Select(permission => permission.Operation);
        operationsRemovedAsValue = JsonSerializer.Serialize(operationsRemoved);

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(Handle_PermissionsExists_RemovesPermissions)}")
        )
        {
            await context.AccountProviderLegalEntities.AddAsync(accountProviderLegalEntity, _cancellationToken);
            await context.SaveChangesAsync(_cancellationToken);

            permissionsBeforeRemovalCount = await context.Permissions.CountAsync(a => a.AccountProviderLegalEntityId == accountProviderLegalEntity.Id, cancellationToken: _cancellationToken);

            RemovePermissionsCommandHandler sut = CreateRemovePermissionsCommandHandler(
                accountProviderLegalEntitiesReadRepositoryMock.Object,
                context,
                _messageSessionMock.Object);


            await sut.Handle(command, _cancellationToken);

            permissionsAfterRemovalCount = await context.Permissions.CountAsync(a => a.AccountProviderLegalEntityId == accountProviderLegalEntity.Id, cancellationToken: _cancellationToken);
            audit = await context.PermissionsAudit.FirstOrDefaultAsync(a => a.AccountLegalEntityId == command.AccountLegalEntityId && a.Ukprn == command.Ukprn!.Value, cancellationToken: _cancellationToken);
        }

        using (new AssertionScope())
        {
            audit.Should().NotBeNull("Audit must have been recorded.");
            audit?.Action.Should().Be(PermissionAction.PermissionDeleted.ToString(), "Audit action must equal PermissionDeleted.");
            audit?.Operations.Should().Be(operationsRemovedAsValue, "Operations removed audit does not match operations removed");
            permissionsBeforeRemovalCount.Should().BeGreaterThan(0, "Permissions before removal should be greater than 0");
            permissionsAfterRemovalCount.Should().Be(0, "Permissions after removal should be 0");
        }

        _messageSessionMock.Verify(m => m.Publish(It.Is<object>(o => o.GetType() == typeof(UpdatedPermissionsEvent)), It.IsAny<PublishOptions>(), It.IsAny<CancellationToken>()));
    }

    private static RemovePermissionsCommandHandler CreateRemovePermissionsCommandHandler(
        IAccountProviderLegalEntitiesReadRepository accountProviderLegalEntitiesReadRepository,
        ProviderRelationshipsDataContext context,
        IMessageSession messageSession)
    {
        PermissionsWriteRepository permissionsWriteRepository = new(context);
        PermissionsAuditWriteRepository permissionsAuditWriteRepository = new(context);

        return new RemovePermissionsCommandHandler(
            accountProviderLegalEntitiesReadRepository,
            permissionsWriteRepository,
            permissionsAuditWriteRepository,
            context,
            messageSession);
    }
}
