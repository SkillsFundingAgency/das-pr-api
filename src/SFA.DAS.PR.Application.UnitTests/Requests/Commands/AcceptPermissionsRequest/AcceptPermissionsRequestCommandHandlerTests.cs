using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.Encoding;
using SFA.DAS.PR.Application.Requests.Commands.AcceptCreateAccountRequest;
using SFA.DAS.PR.Data.UnitTests.Setup;
using SFA.DAS.PR.Data;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.PR.Application.Requests.Commands.AcceptPermissionsRequest;
using SFA.DAS.ProviderRelationships.Messages.Events;

namespace SFA.DAS.PR.Application.UnitTests.Requests.Commands.AcceptPermissionsRequest;

[TestFixture]
public sealed class AcceptPermissionsRequestCommandHandlerTests
{
    private Mock<IProviderRelationshipsDataContext> _providerRelationshipsDataContextMock;
    private Mock<IRequestWriteRepository> _requestWriteRepositoryMock;
    private Mock<IAccountProviderLegalEntitiesWriteRepository> _accountProviderLegalEntitiesWriteRepositoryMock;
    private Mock<IAccountProviderWriteRepository> _accountProviderWriteRepositoryMock;
    private Mock<IPermissionsWriteRepository> _permissionsWriteRepositoryMock;
    private Mock<IMessageSession> _messageSessionMock;
    private Mock<IPermissionsAuditWriteRepository> _permissionsAuditWriteRepositoryMock;
    private Mock<IAccountLegalEntityReadRepository> _accountLegalEntityReadRepositoryMock;

    private AcceptPermissionsRequestCommandHandler _handler;

    [SetUp]
    public void SetUp()
    {
        _providerRelationshipsDataContextMock = new Mock<IProviderRelationshipsDataContext>();
        _accountProviderWriteRepositoryMock = new Mock<IAccountProviderWriteRepository>();
        _accountProviderLegalEntitiesWriteRepositoryMock = new Mock<IAccountProviderLegalEntitiesWriteRepository>();
        _permissionsWriteRepositoryMock = new Mock<IPermissionsWriteRepository>();
        _permissionsAuditWriteRepositoryMock = new Mock<IPermissionsAuditWriteRepository>();
        _messageSessionMock = new Mock<IMessageSession>();
        _requestWriteRepositoryMock = new Mock<IRequestWriteRepository>();

        _handler = new AcceptPermissionsRequestCommandHandler(
            _providerRelationshipsDataContextMock.Object,
            _requestWriteRepositoryMock.Object,
            _accountProviderLegalEntitiesWriteRepositoryMock.Object,
            _accountProviderWriteRepositoryMock.Object,
            _accountLegalEntityReadRepositoryMock.Object,
            _permissionsWriteRepositoryMock.Object,
            _messageSessionMock.Object,
            _permissionsAuditWriteRepositoryMock.Object
        );
    }

    [Test]
    [MoqAutoData]
    public async Task AcceptCreateAccountRequestCommandHanler_ShouldCreateEntities_IfNotExists(AcceptPermissionsRequestCommand command)
    {
        Account account = AccountTestData.CreateAccount(10001);

        command.ActionedBy = Guid.NewGuid().ToString();

        var request = RequestTestData.Create(Guid.NewGuid());

        _requestWriteRepositoryMock.Setup(x => x.GetRequest(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(request);

        _accountProviderWriteRepositoryMock.Setup(x => x.CreateAccountProvider(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AccountProvider());

        _accountProviderLegalEntitiesWriteRepositoryMock.Setup(a => a.CreateAccountProviderLegalEntity(It.IsAny<long>(), It.IsAny<AccountProvider>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(AccountProviderLegalEntityTestData.CreateAccountProviderLegalEntity(account));

        var result = await _handler.Handle(command, CancellationToken.None);

        _accountProviderWriteRepositoryMock.Verify(x => x.CreateAccountProvider(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Once);
        _permissionsWriteRepositoryMock.Verify(x => x.CreatePermissions(It.IsAny<IEnumerable<Permission>>()), Times.Once);
        _providerRelationshipsDataContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(3));
        _messageSessionMock.Verify(m => m.Publish(It.IsAny<UpdatedPermissionsEvent>(), It.IsAny<PublishOptions>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    [MoqAutoData]
    public void AcceptCreateAccountRequestCommandHanler_ShouldGetEntities_IfExists(AcceptPermissionsRequestCommand command)
    {
        Account account = AccountTestData.CreateAccount(1);

        AccountProviderLegalEntity accountProviderLegalEntity =
            AccountProviderLegalEntityTestData.CreateAccountProviderLegalEntity(account);

        command.ActionedBy = Guid.NewGuid().ToString();
        var request = RequestTestData.Create(Guid.NewGuid());

        _requestWriteRepositoryMock.Setup(x => x.GetRequest(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(request);

        _accountProviderWriteRepositoryMock.Setup(x => x.CreateAccountProvider(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new DbUpdateException());

        _accountProviderWriteRepositoryMock.Setup(x => x.GetAccountProvider(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AccountProvider());

        _accountProviderLegalEntitiesWriteRepositoryMock.Setup(x => x.CreateAccountProviderLegalEntity(It.IsAny<long>(), It.IsAny<AccountProvider>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(accountProviderLegalEntity);

        Assert.DoesNotThrowAsync(() => _handler.Handle(command, CancellationToken.None));
        _accountProviderWriteRepositoryMock.Verify(x => x.GetAccountProvider(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Once);
        _messageSessionMock.Verify(m => m.Publish(It.IsAny<AddedAccountProviderEvent>(), It.IsAny<PublishOptions>(), It.IsAny<CancellationToken>()), Times.Never);
        _messageSessionMock.Verify(m => m.Publish(It.IsAny<UpdatedPermissionsEvent>(), It.IsAny<PublishOptions>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    [MoqAutoData]
    public void AcceptCreateAccountRequestCommandHanler_CreatesPermissionAudit(AcceptPermissionsRequestCommand command)
    {
        Account account = AccountTestData.CreateAccount(1);

        AccountProviderLegalEntity accountProviderLegalEntity =
            AccountProviderLegalEntityTestData.CreateAccountProviderLegalEntity(account);

        var request = RequestTestData.Create(Guid.NewGuid());

        _requestWriteRepositoryMock.Setup(x => x.GetRequest(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(request);

        var actionedBy = Guid.NewGuid();
        command.ActionedBy = actionedBy.ToString();

        _requestWriteRepositoryMock.Setup(x => x.GetRequest(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(request);

        _accountProviderWriteRepositoryMock.Setup(x => x.GetAccountProvider(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new AccountProvider());

        _accountProviderLegalEntitiesWriteRepositoryMock.Setup(x => x.CreateAccountProviderLegalEntity(It.IsAny<long>(), It.IsAny<AccountProvider>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(accountProviderLegalEntity);

        Assert.DoesNotThrowAsync(() => _handler.Handle(command, CancellationToken.None));

        _permissionsAuditWriteRepositoryMock.Verify(a => a.RecordPermissionsAudit(
            It.Is<PermissionsAudit>(p =>
                p.Action == nameof(RequestAction.AccountCreated) &&
                p.Ukprn == request.Ukprn &&
                p.AccountLegalEntityId == accountProviderLegalEntity.AccountLegalEntityId &&
                p.EmployerUserRef == actionedBy),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}
