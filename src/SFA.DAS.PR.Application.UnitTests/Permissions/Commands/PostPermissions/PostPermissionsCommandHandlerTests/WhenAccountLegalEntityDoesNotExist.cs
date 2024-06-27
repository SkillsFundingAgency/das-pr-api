using AutoFixture;
using Moq;
using SFA.DAS.PR.Application.Permissions.Commands.PostPermissions;
using SFA.DAS.PR.Data;
using SFA.DAS.PR.Domain.Common;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Application.UnitTests.Permissions.Commands.PostPermissions.PostPermissionsCommandHandlerTests;

public class WhenAccountLegalEntityDoesNotExist
{
    IFixture _fixture;
    PostPermissionsCommand _command;
    CancellationToken _cancellationToken;
    Mock<IAccountProviderLegalEntitiesReadRepository> _accountProviderLegalEntitiesReadRepositoryMock;
    Mock<IAccountLegalEntityReadRepository> _accountLegalEntityReadRepositoryMock;
    Mock<IAccountProviderWriteRepository> _accountProviderWriteRepositoryMock;
    Mock<IAccountProviderLegalEntitiesWriteRepository> _accountProviderLegalEntitiesWriteRepositoryMock;
    AccountLegalEntity _accountLegalEntity;
    Mock<IPermissionsWriteRepository> _permissionsWriteRepositoryMock;
    Mock<IPermissionsAuditWriteRepository> _permissionsAuditWriteRepositoryMock;
    Mock<IProviderRelationshipsDataContext> _providerRelationshipsDataContextMock;
    Mock<IMessageSession> _messageSessionMock;
    PostPermissionsCommandHandler _sut;

    [SetUp]
    public async Task Initialize()
    {
        _fixture = FixtureBuilder.RecursiveMoqFixtureFactory();

        _command = _fixture.Create<PostPermissionsCommand>();

        _cancellationToken = new CancellationToken();

        _accountProviderLegalEntitiesReadRepositoryMock = _fixture.Freeze<Mock<IAccountProviderLegalEntitiesReadRepository>>();
        _accountProviderLegalEntitiesReadRepositoryMock.Setup(a => a.GetAccountProviderLegalEntity(_command.Ukprn, _command.AccountLegalEntityId, _cancellationToken)).ReturnsAsync(() => null);

        _accountLegalEntityReadRepositoryMock = _fixture.Freeze<Mock<IAccountLegalEntityReadRepository>>();
        _accountLegalEntity = _fixture.Create<AccountLegalEntity>();
        _accountLegalEntityReadRepositoryMock.Setup(a => a.GetAccountLegalEntity(_command.AccountLegalEntityId, _cancellationToken)).ReturnsAsync(_accountLegalEntity);

        _accountProviderWriteRepositoryMock = _fixture.Freeze<Mock<IAccountProviderWriteRepository>>();

        _accountProviderLegalEntitiesWriteRepositoryMock = _fixture.Freeze<Mock<IAccountProviderLegalEntitiesWriteRepository>>();

        _permissionsWriteRepositoryMock = _fixture.Freeze<Mock<IPermissionsWriteRepository>>();

        _permissionsAuditWriteRepositoryMock = _fixture.Freeze<Mock<IPermissionsAuditWriteRepository>>();

        _providerRelationshipsDataContextMock = _fixture.Freeze<Mock<IProviderRelationshipsDataContext>>();

        _messageSessionMock = _fixture.Freeze<Mock<IMessageSession>>();

        _sut = _fixture.Create<PostPermissionsCommandHandler>();

        await _sut.Handle(_command, _cancellationToken);
    }

    [Test]
    public void ThenInvokesAccountProviderLegalEntitiesReadRepository_ToCheckExistingRelationship()
    {
        _accountProviderLegalEntitiesReadRepositoryMock.Verify(r => r.GetAccountProviderLegalEntity(_command.Ukprn, _command.AccountLegalEntityId, _cancellationToken), Times.Once);
    }

    [Test]
    public void ThenInvokesAccountLegalEntityReadRepository_ToGetAccountLegalEntity()
    {
        _accountLegalEntityReadRepositoryMock.Verify(a => a.GetAccountLegalEntity(_command.AccountLegalEntityId, _cancellationToken), Times.Once);
    }

    [Test]
    public void ThenInvokesAccountProviderLegalEntitiesWriteRepository_ToCreateAccountProviderLegalEntityRelationship()
    {
        _accountProviderLegalEntitiesWriteRepositoryMock.Verify(a => a.CreateAccountProviderLegalEntity(_command.AccountLegalEntityId, It.IsAny<AccountProvider>(), _cancellationToken), Times.Once);
    }

    [Test]
    public void ThenInvokesPermissionsWriteRepository_ToAddPermissions()
    {
        _permissionsWriteRepositoryMock.Verify(p => p.CreatePermissions(It.IsAny<IEnumerable<Permission>>()), Times.Once);
    }

    [Test]
    public void ThenInvokesPermissionAuditWriteRepository_ToAddAudit()
    {
        _permissionsAuditWriteRepositoryMock.Verify(a => a.RecordPermissionsAudit(
            It.Is<PermissionsAudit>(p =>
                p.Action == PermissionAuditActions.PermissionCreatedAction &&
                p.Ukprn == _command.Ukprn &&
                p.AccountLegalEntityId == _command.AccountLegalEntityId &&
                p.EmployerUserRef == _command.UserRef),
            _cancellationToken), Times.Once);
    }

    [Test]
    public void ThenInvokesSaveChangesOnDataContext()
    {
        _providerRelationshipsDataContextMock.Verify(p => p.SaveChangesAsync(_cancellationToken), Times.Once);
    }

    [Test]
    public async Task AndAccountProviderRelationshipExists_ThenDoesNotInvokeCreateAccountProvider()
    {
        _accountProviderWriteRepositoryMock = _fixture.Freeze<Mock<IAccountProviderWriteRepository>>();
        _accountProviderWriteRepositoryMock.Setup(a => a.GetAccountProvider(_command.Ukprn, _accountLegalEntity.AccountId, _cancellationToken)).ReturnsAsync(_fixture.Create<AccountProvider>());

        _sut = _fixture.Create<PostPermissionsCommandHandler>();
        await _sut.Handle(_command, _cancellationToken);

        _accountProviderWriteRepositoryMock.Verify(a => a.CreateAccountProvider(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Never);
        _messageSessionMock.Verify(m => m.Publish(It.IsAny<object>(), It.IsAny<PublishOptions>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    public async Task AndAccountProviderRelationshipDoesNotExists_ThenInvokesCreateAccountProvider()
    {
        _accountProviderWriteRepositoryMock = _fixture.Freeze<Mock<IAccountProviderWriteRepository>>();
        _accountProviderWriteRepositoryMock.Setup(a => a.GetAccountProvider(_command.Ukprn, _accountLegalEntity.AccountId, _cancellationToken)).ReturnsAsync(() => null);

        _sut = _fixture.Create<PostPermissionsCommandHandler>();
        await _sut.Handle(_command, _cancellationToken);

        _accountProviderWriteRepositoryMock.Verify(a => a.CreateAccountProvider(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Once);
        _messageSessionMock.Verify(m => m.Publish(It.IsAny<object>(), It.IsAny<PublishOptions>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
