using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using SFA.DAS.Encoding;
using SFA.DAS.PR.Application.Requests.Commands.AcceptCreateAccountRequest;
using SFA.DAS.PR.Data;
using SFA.DAS.PR.Data.UnitTests.Setup;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;

namespace SFA.DAS.PR.Application.UnitTests.Requests.Commands.AcceptCreateAccountRequest
{
    [TestFixture]
    public class AcceptCreateAccountRequestCommandHandlerTests
    {
        private Mock<ILogger<AcceptCreateAccountRequestCommandHandler>> _loggerMock;
        private Mock<IEncodingService> _encodingServiceMock;
        private Mock<IProviderRelationshipsDataContext> _providerRelationshipsDataContextMock;
        private Mock<IAccountWriteRepository> _accountWriteRepositoryMock;
        private Mock<IAccountReadRepository> _accountReadRepositoryMock;
        private Mock<IAccountProviderWriteRepository> _accountProviderWriteRepositoryMock;
        private Mock<IAccountLegalEntityReadRepository> _accountLegalEntityReadRepositoryMock;
        private Mock<IAccountLegalEntityWriteRepository> _accountLegalEntityWriteRepositoryMock;
        private Mock<IAccountProviderLegalEntitiesWriteRepository> _accountProviderLegalEntitiesWriteRepositoryMock;
        private Mock<IPermissionsWriteRepository> _permissionsWriteRepositoryMock;
        private Mock<IPermissionsAuditWriteRepository> _permissionsAuditWriteRepositoryMock;
        private Mock<IMessageSession> _messageSessionMock;
        private Mock<IRequestReadRepository> _requestReadRepositoryMock;

        private AcceptCreateAccountRequestCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<AcceptCreateAccountRequestCommandHandler>>();
            _encodingServiceMock = new Mock<IEncodingService>();
            _providerRelationshipsDataContextMock = new Mock<IProviderRelationshipsDataContext>();
            _accountWriteRepositoryMock = new Mock<IAccountWriteRepository>();
            _accountReadRepositoryMock = new Mock<IAccountReadRepository>();
            _accountProviderWriteRepositoryMock = new Mock<IAccountProviderWriteRepository>();
            _accountLegalEntityReadRepositoryMock = new Mock<IAccountLegalEntityReadRepository>();
            _accountLegalEntityWriteRepositoryMock = new Mock<IAccountLegalEntityWriteRepository>();
            _accountProviderLegalEntitiesWriteRepositoryMock = new Mock<IAccountProviderLegalEntitiesWriteRepository>();
            _permissionsWriteRepositoryMock = new Mock<IPermissionsWriteRepository>();
            _permissionsAuditWriteRepositoryMock = new Mock<IPermissionsAuditWriteRepository>();
            _messageSessionMock = new Mock<IMessageSession>();
            _requestReadRepositoryMock = new Mock<IRequestReadRepository>();

            _handler = new AcceptCreateAccountRequestCommandHandler(
                _loggerMock.Object,
                _encodingServiceMock.Object,
                _providerRelationshipsDataContextMock.Object,
                _accountWriteRepositoryMock.Object,
                _accountReadRepositoryMock.Object,
                _accountProviderWriteRepositoryMock.Object,
                _accountLegalEntityReadRepositoryMock.Object,
                _accountLegalEntityWriteRepositoryMock.Object,
                _accountProviderLegalEntitiesWriteRepositoryMock.Object,
                _permissionsWriteRepositoryMock.Object,
                _permissionsAuditWriteRepositoryMock.Object,
                _messageSessionMock.Object,
                _requestReadRepositoryMock.Object
            );
        }

        [Test]
        [MoqAutoData]
        public async Task AcceptCreateAccountRequestCommandHanler_ShouldCreateEntities_IfNotExists(AcceptCreateAccountRequestCommand command)
        {
            Account account = AccountTestData.CreateAccount(10001);

            command.ActionedBy = Guid.NewGuid().ToString();

            var request = RequestTestData.Create(Guid.NewGuid());

            _requestReadRepositoryMock.Setup(x => x.GetRequest(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(request);

            _accountWriteRepositoryMock.Setup(x => x.CreateAccount(It.IsAny<Account>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Account());

            _accountLegalEntityWriteRepositoryMock.Setup(x => x.CreateAccountLegalEntity(It.IsAny<AccountLegalEntity>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AccountLegalEntity());

            _accountProviderWriteRepositoryMock.Setup(x => x.CreateAccountProvider(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AccountProvider());

            _accountProviderLegalEntitiesWriteRepositoryMock.Setup(a => a.CreateAccountProviderLegalEntity(It.IsAny<long>(), It.IsAny<AccountProvider>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(AccountProviderLegalEntityTestData.CreateAccountProviderLegalEntity(account));

            var result = await _handler.Handle(command, CancellationToken.None);

            _accountWriteRepositoryMock.Verify(x => x.CreateAccount(It.IsAny<Account>(), It.IsAny<CancellationToken>()), Times.Once);
            _accountLegalEntityWriteRepositoryMock.Verify(x => x.CreateAccountLegalEntity(It.IsAny<AccountLegalEntity>(), It.IsAny<CancellationToken>()), Times.Once);
            _accountProviderWriteRepositoryMock.Verify(x => x.CreateAccountProvider(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Once);
            _permissionsWriteRepositoryMock.Verify(x => x.CreatePermissions(It.IsAny<IEnumerable<Permission>>()), Times.Once);
            _providerRelationshipsDataContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(3));
            _messageSessionMock.Verify(m => m.Publish(It.IsAny<AddedAccountProviderEvent>(), It.IsAny<PublishOptions>(), It.IsAny<CancellationToken>()), Times.Once);
            _messageSessionMock.Verify(m => m.Publish(It.IsAny<UpdatedPermissionsEvent>(), It.IsAny<PublishOptions>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        [MoqAutoData]
        public void AcceptCreateAccountRequestCommandHanler_ShouldGetEntities_IfExists(AcceptCreateAccountRequestCommand command)
        {
            Account account = AccountTestData.CreateAccount(1);

            AccountProviderLegalEntity accountProviderLegalEntity = 
                AccountProviderLegalEntityTestData.CreateAccountProviderLegalEntity(account);

            command.ActionedBy = Guid.NewGuid().ToString();
            var request = RequestTestData.Create(Guid.NewGuid());

            _requestReadRepositoryMock.Setup(x => x.GetRequest(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(request);

            _accountWriteRepositoryMock.Setup(x => x.CreateAccount(It.IsAny<Account>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new DbUpdateException());

            _accountProviderWriteRepositoryMock.Setup(x => x.CreateAccountProvider(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new DbUpdateException());

            _accountLegalEntityWriteRepositoryMock.Setup(x => x.CreateAccountLegalEntity(It.IsAny<AccountLegalEntity>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new DbUpdateException());

            _accountReadRepositoryMock.Setup(x => x.GetAccount(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Account());

            _accountProviderWriteRepositoryMock.Setup(x => x.GetAccountProvider(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AccountProvider());

            _accountProviderLegalEntitiesWriteRepositoryMock.Setup(x => x.CreateAccountProviderLegalEntity(It.IsAny<long>(), It.IsAny<AccountProvider>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(accountProviderLegalEntity);

            Assert.DoesNotThrowAsync(() => _handler.Handle(command, CancellationToken.None));
            _accountReadRepositoryMock.Verify(x => x.GetAccount(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Once);
            _accountProviderWriteRepositoryMock.Verify(x => x.GetAccountProvider(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Once);
            _accountLegalEntityReadRepositoryMock.Verify(x => x.GetAccountLegalEntity(It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Once);
            _messageSessionMock.Verify(m => m.Publish(It.IsAny<AddedAccountProviderEvent>(), It.IsAny<PublishOptions>(), It.IsAny<CancellationToken>()), Times.Never);
            _messageSessionMock.Verify(m => m.Publish(It.IsAny<UpdatedPermissionsEvent>(), It.IsAny<PublishOptions>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        [MoqAutoData]
        public void AcceptCreateAccountRequestCommandHanler_CreatesPermissionAudit(AcceptCreateAccountRequestCommand command)
        {
            Account account = AccountTestData.CreateAccount(1);

            AccountProviderLegalEntity accountProviderLegalEntity =
                AccountProviderLegalEntityTestData.CreateAccountProviderLegalEntity(account);

            var request = RequestTestData.Create(Guid.NewGuid());

            _requestReadRepositoryMock.Setup(x => x.GetRequest(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(request);

            var actionedBy = Guid.NewGuid();
            command.ActionedBy = actionedBy.ToString();

            _requestReadRepositoryMock.Setup(x => x.GetRequest(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(request);

            _accountReadRepositoryMock.Setup(x => x.GetAccount(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new Account());

            _accountProviderWriteRepositoryMock.Setup(x => x.GetAccountProvider(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AccountProvider());

            _accountProviderLegalEntitiesWriteRepositoryMock.Setup(x => x.CreateAccountProviderLegalEntity(It.IsAny<long>(), It.IsAny<AccountProvider>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(accountProviderLegalEntity);

            Assert.DoesNotThrowAsync(() => _handler.Handle(command, CancellationToken.None));

            _permissionsAuditWriteRepositoryMock.Verify(a => a.RecordPermissionsAudit(
                It.Is<PermissionsAudit>(p =>
                    p.Action == nameof(RequestAction.AccountCreated) &&
                    p.Ukprn == request.Ukprn &&
                    p.AccountLegalEntityId == command.AccountLegalEntity.Id &&
                    p.EmployerUserRef == actionedBy),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
