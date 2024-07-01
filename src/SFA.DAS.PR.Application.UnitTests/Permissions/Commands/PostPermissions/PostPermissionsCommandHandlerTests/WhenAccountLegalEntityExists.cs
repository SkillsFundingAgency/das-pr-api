using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.PR.Application.Permissions.Commands.PostPermissions;
using SFA.DAS.PR.Data;
using SFA.DAS.PR.Domain.Common;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Application.UnitTests.Permissions.Commands.PostPermissions.PostPermissionsCommandHandlerTests;

public class WhenAccountLegalEntityExists
{
    [Test, RecursiveMoqAutoData]
    public async Task Handle_DifferentPermissions_UpdatesPermission(
        [Frozen] Mock<IAccountProviderLegalEntitiesReadRepository> accountProviderLegalEntitiesReadRepositoryMock,
        [Frozen] Mock<IPermissionsWriteRepository> permissionsWriteRepositoryMock,
        [Frozen] Mock<IProviderRelationshipsDataContext> providerRelationshipsDataContextMock,
        [Frozen] Mock<IPermissionsAuditWriteRepository> permissionsAuditWriteRepositoryMock,
        [Frozen] Mock<IMessageSession> messageSessionMock,
        PostPermissionsCommandHandler sut,
        PostPermissionsCommand command,
        AccountProviderLegalEntity accountProviderLegalEntity,
        CancellationToken cancellationToken)
    {
        command.Operations = new() { Operation.CreateCohort };

        accountProviderLegalEntity.Permissions = new List<Permission> {
            new Permission { AccountProviderLegalEntityId = command.AccountLegalEntityId, Operation = Operation.RecruitmentRequiresReview }
        };

        accountProviderLegalEntitiesReadRepositoryMock.Setup(a => a.GetAccountProviderLegalEntity(command.Ukprn, command.AccountLegalEntityId, cancellationToken)).ReturnsAsync(accountProviderLegalEntity);


        var response = await sut.Handle(command, cancellationToken);


        response.IsValidResponse.Should().BeTrue();

        permissionsWriteRepositoryMock.Verify(p => p.DeletePermissions(It.Is<IEnumerable<Permission>>(x => x.Count() == 1 && x.First().Operation == Operation.RecruitmentRequiresReview)), Times.Once);

        permissionsWriteRepositoryMock.Verify(p => p.CreatePermissions(It.Is<IEnumerable<Permission>>(x => x.Count() == 1 && x.First().Operation == Operation.CreateCohort)), Times.Once);

        permissionsAuditWriteRepositoryMock.Verify(a => a.RecordPermissionsAudit(It.Is<PermissionsAudit>(p => p.Ukprn == command.Ukprn && p.Action == PermissionAuditActions.PermissionUpdatedAction && p.AccountLegalEntityId == command.AccountLegalEntityId && p.EmployerUserRef == command.UserRef), cancellationToken), Times.Once);

        providerRelationshipsDataContextMock.Verify(d => d.SaveChangesAsync(cancellationToken), Times.Once);

        messageSessionMock.Verify(m => m.Publish(It.IsAny<UpdatedPermissionsEvent>(), It.IsAny<PublishOptions>(), cancellationToken), Times.Once);
    }

    [Test, RecursiveMoqAutoData]
    public async Task Handle_SimilarPermissions_NoUpdatesToPermission(
        [Frozen] Mock<IAccountProviderLegalEntitiesReadRepository> accountProviderLegalEntitiesReadRepositoryMock,
        [Frozen] Mock<IPermissionsWriteRepository> permissionsWriteRepositoryMock,
        [Frozen] Mock<IPermissionsAuditWriteRepository> permissionsAuditWriteRepositoryMock,
        [Frozen] Mock<IProviderRelationshipsDataContext> _providerRelationshipsDataContextMock,
        [Frozen] Mock<IMessageSession> messageSessionMock,
        PostPermissionsCommandHandler sut,
        PostPermissionsCommand command,
        AccountProviderLegalEntity accountProviderLegalEntity,
        CancellationToken cancellationToken)
    {
        accountProviderLegalEntity.Permissions = command.Operations.Select(c => new Permission { AccountProviderLegalEntityId = command.AccountLegalEntityId, Operation = c }).ToList();

        accountProviderLegalEntitiesReadRepositoryMock.Setup(a => a.GetAccountProviderLegalEntity(command.Ukprn, command.AccountLegalEntityId, cancellationToken)).ReturnsAsync(accountProviderLegalEntity);


        var response = await sut.Handle(command, cancellationToken);


        response.IsValidResponse.Should().BeTrue();

        permissionsWriteRepositoryMock.Verify(p => p.DeletePermissions(It.IsAny<IEnumerable<Permission>>()), Times.Never);

        permissionsWriteRepositoryMock.Verify(p => p.CreatePermissions(It.IsAny<IEnumerable<Permission>>()), Times.Never);

        permissionsAuditWriteRepositoryMock.Verify(a => a.RecordPermissionsAudit(It.IsAny<PermissionsAudit>(), It.IsAny<CancellationToken>()), Times.Never);

        _providerRelationshipsDataContextMock.Verify(d => d.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

        messageSessionMock.Verify(m => m.Publish(It.IsAny<UpdatedPermissionsEvent>(), It.IsAny<PublishOptions>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
