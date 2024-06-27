using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Application.Permissions.Commands.PostPermissions;
using SFA.DAS.PR.Data;
using SFA.DAS.PR.Data.Repositories;
using SFA.DAS.PR.Data.UnitTests.InMemoryDatabases;
using SFA.DAS.PR.Data.UnitTests.Setup;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Application.UnitTests.Permissions.Commands.PostPermissions.PostPermissionsCommandHandlerTests;

public class PostPermissionsCommandHandlerIntegrationTests
{
    private readonly CancellationToken cancellationToken = CancellationToken.None;

    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_PostPermissions_Indentical_Permissions_Returns_PostPermissionsCommandResult(
        [Frozen] Mock<IAccountProviderLegalEntitiesReadRepository> accountProviderLegalEntitiesReadRepository,
        [Frozen] Mock<IAccountLegalEntityReadRepository> accountLegalEntityReadRepository,
        PostPermissionsCommandHandler sut,
        AccountProviderLegalEntity accountProviderLegalEntity,
        AccountLegalEntity accountLegalEntity,
        PostPermissionsCommand command,
        CancellationToken cancellationToken
    )
    {
        accountProviderLegalEntitiesReadRepository.Setup(a =>
            a.GetAccountProviderLegalEntity(
                command.Ukprn,
                command.AccountLegalEntityId,
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(accountProviderLegalEntity);

        accountLegalEntityReadRepository.Setup(a =>
            a.GetAccountLegalEntity(command.AccountLegalEntityId, cancellationToken)
        ).ReturnsAsync(accountLegalEntity);

        ValidatedResponse<PostPermissionsCommandResult> result = await sut.Handle(command, cancellationToken);

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<PostPermissionsCommandResult>();
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_PostPermissions_Remove_Permissions_Returns_PostPermissionsCommandResult(
        [Frozen] Mock<IAccountProviderLegalEntitiesReadRepository> accountProviderLegalEntitiesReadRepository,
        [Frozen] Mock<IAccountLegalEntityReadRepository> accountLegalEntityReadRepository,
        AccountProviderLegalEntity accountProviderLegalEntity,
        AccountLegalEntity accountLegalEntity,
        PostPermissionsCommand command
    )
    {
        command.Operations = new() { Operation.CreateCohort };

        accountProviderLegalEntitiesReadRepository.Setup(a =>
            a.GetAccountProviderLegalEntity(
                command.Ukprn,
                command.AccountLegalEntityId,
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(accountProviderLegalEntity);

        accountLegalEntityReadRepository.Setup(a =>
            a.GetAccountLegalEntity(command.AccountLegalEntityId, cancellationToken)
        ).ReturnsAsync(accountLegalEntity);

        ValidatedResponse<PostPermissionsCommandResult> result = null!;

        int permissionCount = 0;

        PermissionsAudit? audit = null;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(Handle_PostPermissions_Remove_Permissions_Returns_PostPermissionsCommandResult)}")
        )
        {
            await context.AccountProviderLegalEntities.AddAsync(accountProviderLegalEntity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            PostPermissionsCommandHandler sut = CreatePostPermissionsCommandHandler(
               accountProviderLegalEntitiesReadRepository.Object,
               accountLegalEntityReadRepository.Object,
               context
            );

            result = await sut.Handle(command, cancellationToken);

            permissionCount = await context.Permissions.CountAsync(a => a.AccountProviderLegalEntityId == accountProviderLegalEntity.Id);
            audit = await context.PermissionsAudit.FirstOrDefaultAsync(a => a.AccountLegalEntityId == command.AccountLegalEntityId && a.Ukprn == command.Ukprn!.Value);
        }

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<PostPermissionsCommandResult>();

        Assert.Multiple(() =>
        {
            Assert.That(audit, Is.Not.Null, "Audit must have been recorded.");
            Assert.That(audit?.Action, Is.EqualTo("Updated"), "Audit action must equal updated.");
            Assert.That(command.Operations, Has.Count.EqualTo(permissionCount), "Permissions after removal should be equal to the passed permissions count.");
        });
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_PostPermissions_Add_Permissions_Returns_PostPermissionsCommandResult(
        [Frozen] Mock<IAccountProviderLegalEntitiesReadRepository> accountProviderLegalEntitiesReadRepository,
        [Frozen] Mock<IAccountLegalEntityReadRepository> accountLegalEntityReadRepository,
        AccountProviderLegalEntity accountProviderLegalEntity,
        AccountLegalEntity accountLegalEntity,
        PostPermissionsCommand command
    )
    {
        command.Operations = new() { Operation.CreateCohort, Operation.RecruitmentRequiresReview };

        accountProviderLegalEntity.Permissions = new();

        accountProviderLegalEntitiesReadRepository.Setup(a =>
            a.GetAccountProviderLegalEntity(
                command.Ukprn,
                command.AccountLegalEntityId,
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(accountProviderLegalEntity);

        accountLegalEntityReadRepository.Setup(a =>
            a.GetAccountLegalEntity(command.AccountLegalEntityId, cancellationToken)
        ).ReturnsAsync(accountLegalEntity);

        ValidatedResponse<PostPermissionsCommandResult> result = null!;

        int permissionCount = 0;

        PermissionsAudit? audit = null;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(Handle_PostPermissions_Add_Permissions_Returns_PostPermissionsCommandResult)}")
        )
        {
            await context.AccountProviderLegalEntities.AddAsync(accountProviderLegalEntity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            PostPermissionsCommandHandler sut = CreatePostPermissionsCommandHandler(
               accountProviderLegalEntitiesReadRepository.Object,
               accountLegalEntityReadRepository.Object,
               context
           );

            result = await sut.Handle(command, cancellationToken);

            permissionCount = await context.Permissions.CountAsync(a => a.AccountProviderLegalEntityId == accountProviderLegalEntity.Id);
            audit = await context.PermissionsAudit.FirstOrDefaultAsync(a => a.AccountLegalEntityId == command.AccountLegalEntityId && a.Ukprn == command.Ukprn!.Value);
        }

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<PostPermissionsCommandResult>();

        Assert.Multiple(() =>
        {
            Assert.That(audit, Is.Not.Null, "Audit must have been recorded.");
            Assert.That(audit?.Action, Is.EqualTo("Updated"), "Audit action must equal updated.");
            Assert.That(command.Operations, Has.Count.EqualTo(permissionCount), "Added permissions should be equal to the passed permissions count.");
        });
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_PostPermissions_Create_Permissions_Returns_PostPermissionsCommandResult(
        [Frozen] Mock<IAccountProviderLegalEntitiesReadRepository> accountProviderLegalEntitiesReadRepository,
        [Frozen] Mock<IAccountLegalEntityReadRepository> accountLegalEntityReadRepository,
        PostPermissionsCommand command
    )
    {
        AccountLegalEntity accountLegalEntity = AccountLegalEntityTestData.CreateAccountLegalEntity();
        command.AccountLegalEntityId = accountLegalEntity.Id;

        accountProviderLegalEntitiesReadRepository.Setup(a =>
            a.GetAccountProviderLegalEntity(
                command.Ukprn,
                command.AccountLegalEntityId,
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync((AccountProviderLegalEntity?)null);

        accountLegalEntityReadRepository.Setup(a =>
            a.GetAccountLegalEntity(command.AccountLegalEntityId, cancellationToken)
        ).ReturnsAsync(accountLegalEntity);

        ValidatedResponse<PostPermissionsCommandResult> result = null!;

        int permissionCount = 0;

        PermissionsAudit? audit = null;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(Handle_PostPermissions_Create_Permissions_Returns_PostPermissionsCommandResult)}")
        )
        {
            await context.AccountLegalEntities.AddAsync(accountLegalEntity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            PostPermissionsCommandHandler sut = CreatePostPermissionsCommandHandler(
                accountProviderLegalEntitiesReadRepository.Object,
                accountLegalEntityReadRepository.Object,
                context
            );

            result = await sut.Handle(command, cancellationToken);

            AccountProviderLegalEntity? legalEntity = await context.AccountProviderLegalEntities.FirstOrDefaultAsync(a => a.AccountLegalEntityId == command.AccountLegalEntityId);

            permissionCount = await context.Permissions.CountAsync(a => a.AccountProviderLegalEntityId == legalEntity!.Id);
            audit = await context.PermissionsAudit.FirstOrDefaultAsync(a => a.AccountLegalEntityId == command.AccountLegalEntityId && a.Ukprn == command.Ukprn!.Value);
        }

        result.Result.Should().NotBeNull();
        result.Result.Should().BeOfType<PostPermissionsCommandResult>();

        Assert.Multiple(() =>
        {
            Assert.That(audit, Is.Not.Null, "Audit must have been recorded.");
            Assert.That(audit?.Action, Is.EqualTo("Created"), "Audit action must equal created.");
            Assert.That(command.Operations, Has.Count.EqualTo(permissionCount), "Added permissions should be equal to the passed permissions count.");
        });
    }

    private static PostPermissionsCommandHandler CreatePostPermissionsCommandHandler(
        IAccountProviderLegalEntitiesReadRepository accountProviderLegalEntitiesReadRepository,
        IAccountLegalEntityReadRepository accountLegalEntityReadRepository,
        ProviderRelationshipsDataContext context
    )
    {
        AccountProviderWriteRepository accountProviderWriteRepository = new(context);
        AccountProviderLegalEntitiesWriteRepository accountProviderLegalEntitiesWriteRepository = new(context);
        PermissionsWriteRepository permissionsWriteRepository = new(context);
        PermissionsAuditWriteRepository permissionsAuditWriteRepository = new(context);

        return new(
            accountProviderLegalEntitiesReadRepository,
            accountLegalEntityReadRepository,
            accountProviderWriteRepository,
            accountProviderLegalEntitiesWriteRepository,
            permissionsWriteRepository,
            permissionsAuditWriteRepository,
            context,
            Mock.Of<IMessageSession>()
        );
    }
}
