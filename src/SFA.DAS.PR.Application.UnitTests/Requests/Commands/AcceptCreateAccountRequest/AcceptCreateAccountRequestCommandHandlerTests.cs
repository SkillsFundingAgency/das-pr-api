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

namespace SFA.DAS.PR.Application.UnitTests.Requests.Commands.AcceptCreateAccountRequest
{
    [TestFixture]
    public class AcceptCreateAccountRequestCommandHandlerTests
    {
        private Mock<ILogger<AcceptCreateAccountRequestCommandHandler>> _loggerMock;
        private Mock<IEncodingService> _encodingServiceMock;
        private Mock<IProviderRelationshipsDataContext> _providerRelationshipsDataContextMock;
        private Mock<IAccountWriteRepository> _accountWriteRepositoryMock;
        private Mock<IAccountProviderWriteRepository> _accountProviderWriteRepositoryMock;
        private Mock<IAccountLegalEntityWriteRepository> _accountLegalEntityWriteRepositoryMock;
        private Mock<IAccountProviderLegalEntitiesWriteRepository> _accountProviderLegalEntitiesWriteRepositoryMock;
        private Mock<IPermissionsAuditWriteRepository> _permissionsAuditWriteRepositoryMock;
        private Mock<IMessageSession> _messageSessionMock;
        private Mock<IRequestWriteRepository> _requestWriteRepositoryMock;

        private AcceptCreateAccountRequestCommandHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _loggerMock = new Mock<ILogger<AcceptCreateAccountRequestCommandHandler>>();
            _encodingServiceMock = new Mock<IEncodingService>();
            _providerRelationshipsDataContextMock = new Mock<IProviderRelationshipsDataContext>();
            _accountWriteRepositoryMock = new Mock<IAccountWriteRepository>();
            _accountProviderWriteRepositoryMock = new Mock<IAccountProviderWriteRepository>();
            _accountLegalEntityWriteRepositoryMock = new Mock<IAccountLegalEntityWriteRepository>();
            _accountProviderLegalEntitiesWriteRepositoryMock = new Mock<IAccountProviderLegalEntitiesWriteRepository>();
            _permissionsAuditWriteRepositoryMock = new Mock<IPermissionsAuditWriteRepository>();
            _messageSessionMock = new Mock<IMessageSession>();
            _requestWriteRepositoryMock = new Mock<IRequestWriteRepository>();

            _handler = new AcceptCreateAccountRequestCommandHandler(
                _loggerMock.Object,
                _encodingServiceMock.Object,
                _providerRelationshipsDataContextMock.Object,
                _accountWriteRepositoryMock.Object,
                _accountProviderWriteRepositoryMock.Object,
                _accountLegalEntityWriteRepositoryMock.Object,
                _accountProviderLegalEntitiesWriteRepositoryMock.Object,
                _permissionsAuditWriteRepositoryMock.Object,
                _messageSessionMock.Object,
                _requestWriteRepositoryMock.Object
            );
        }

        [Test]
        [MoqAutoData]
        public async Task AcceptCreateAccountRequestCommandHandler_ShouldCreateEntities_IfNotExists(
            AcceptCreateAccountRequestCommand command
        )
        {
            command.ActionedBy = Guid.NewGuid().ToString();

            Account account = AccountTestData.CreateAccount(10001);

            Request request = RequestTestData.Create(Guid.NewGuid());

            AccountProvider accountProvider = AccountProviderTestData.CreateAccountProvider(1, account.Id, request.Ukprn);

            AccountProviderLegalEntity accountProviderLegalEntity = AccountProviderLegalEntityTestData.CreateAccountProviderLegalEntity(account);

            _requestWriteRepositoryMock.Setup(x => 
                x.GetRequest(
                    command.RequestId,
                    CancellationToken.None
                )
            )
            .ReturnsAsync(request);

            _accountWriteRepositoryMock.Setup(x => 
                x.CreateAccount(
                    It.Is<Account>(a => 
                        a.Id == command.Account.Id &&
                        a.Name == command.Account.Name
                    ),
                    CancellationToken.None
                )
            )
            .ReturnsAsync(new Account());

            _accountLegalEntityWriteRepositoryMock.Setup(x => 
                x.CreateAccountLegalEntity(
                    It.Is<AccountLegalEntity>(a =>
                        a.Id == command.AccountLegalEntity.Id &&
                        a.AccountId == command.Account.Id &&
                        a.Name == command.AccountLegalEntity.Name
                    ),
                    CancellationToken.None
                )
            )
            .ReturnsAsync(new AccountLegalEntity());

            _accountProviderWriteRepositoryMock.Setup(x => 
                x.CreateAccountProvider(
                    request.Ukprn,
                    command.Account.Id, 
                    CancellationToken.None
                )
            )
            .ReturnsAsync(accountProvider);

            var result = await _handler.Handle(command, CancellationToken.None);

            _accountProviderLegalEntitiesWriteRepositoryMock.Verify(a => a.CreateAccountProviderLegalEntity(
               It.Is<AccountProviderLegalEntity>(p =>
                   p.AccountLegalEntityId == command.AccountLegalEntity.Id &&
                   p.Permissions.Count == request.PermissionRequests.Count &&
                   p.AccountProvider == accountProvider),
                   CancellationToken.None),
               Times.Once
            );

            VerifyAccountCreation(command);

            VerifyAccountLegalEntityCreation(command);

            VerifyAccountProviderCreation(request.Ukprn, command.Account.Id);

            _providerRelationshipsDataContextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Exactly(3));

            VerifyProviderAddedEventPublished();

            VerifyPermissionsUpdatedEventPublished();
        }

        [Test]
        [MoqAutoData]
        public void AcceptCreateAccountRequestCommandHanler_ShouldGetEntities_IfExists(AcceptCreateAccountRequestCommand command)
        {
            Account account = AccountTestData.CreateAccount(1);

            AccountProviderLegalEntity accountProviderLegalEntity = 
                AccountProviderLegalEntityTestData.CreateAccountProviderLegalEntity(account);

            AccountProvider accountProvider = AccountProviderTestData.CreateAccountProvider(1001, account.Id, 1001);

            command.ActionedBy = Guid.NewGuid().ToString();
            var request = RequestTestData.Create(Guid.NewGuid());

            _requestWriteRepositoryMock.Setup(x => 
                x.GetRequest(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(request);

            _accountWriteRepositoryMock.Setup(x => 
                x.CreateAccount(
                    It.IsAny<Account>(), 
                    It.IsAny<CancellationToken>()
                )
            )
            .ThrowsAsync(new DbUpdateException());

            _accountProviderWriteRepositoryMock.Setup(x => 
                x.CreateAccountProvider(
                    It.IsAny<long>(),
                    It.IsAny<long>(), 
                    It.IsAny<CancellationToken>()
                )
            )
            .ThrowsAsync(new DbUpdateException());

            _accountLegalEntityWriteRepositoryMock.Setup(x => 
                x.CreateAccountLegalEntity(
                    It.IsAny<AccountLegalEntity>(), 
                    It.IsAny<CancellationToken>()
                )
            )
            .ThrowsAsync(new DbUpdateException());

            _accountProviderWriteRepositoryMock.Setup(x => 
                x.GetAccountProvider(
                    It.IsAny<long>(), 
                    It.IsAny<long>(), 
                    It.IsAny<CancellationToken>()
                )
            )
            .ReturnsAsync(accountProvider);

            Assert.DoesNotThrowAsync(() => _handler.Handle(command, CancellationToken.None));

            _accountProviderLegalEntitiesWriteRepositoryMock.Verify(a => a.CreateAccountProviderLegalEntity(
               It.Is<AccountProviderLegalEntity>(p =>
                   p.AccountLegalEntityId == command.AccountLegalEntity.Id &&
                   p.Permissions.Count == request.PermissionRequests.Count &&
                   p.AccountProvider == accountProvider),
                   CancellationToken.None),
               Times.Once
            );

            _accountProviderWriteRepositoryMock.Verify(x => x.GetAccountProvider(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<CancellationToken>()), Times.Once);

            VerifyPermissionsUpdatedEventPublished();
        }

        [Test]
        [MoqAutoData]
        public void AcceptCreateAccountRequestCommandHanler_CreatesPermissionAudit(AcceptCreateAccountRequestCommand command)
        {
            var actionedBy = Guid.NewGuid();
            command.ActionedBy = actionedBy.ToString();

            Account account = AccountTestData.CreateAccount(1);

            AccountProviderLegalEntity accountProviderLegalEntity = AccountProviderLegalEntityTestData.CreateAccountProviderLegalEntity(account);

            var request = RequestTestData.Create(Guid.NewGuid());

            AccountProvider accountProvider = AccountProviderTestData.CreateAccountProvider(1, account.Id, request.Ukprn);

            _requestWriteRepositoryMock.Setup(x => 
                x.GetRequest(
                    command.RequestId, 
                    CancellationToken.None
                )
            )
            .ReturnsAsync(request);

            _accountProviderWriteRepositoryMock.Setup(x => 
                x.GetAccountProvider(
                    request.Ukprn, 
                    command.Account.Id,
                    CancellationToken.None
                )
            )
            .ReturnsAsync(accountProvider);

            Assert.DoesNotThrowAsync(() => _handler.Handle(command, CancellationToken.None));

            _permissionsAuditWriteRepositoryMock.Verify(a => a.RecordPermissionsAudit(
                    It.Is<PermissionsAudit>(p =>
                        p.Action == nameof(RequestAction.AccountCreated) &&
                        p.Ukprn == request.Ukprn &&
                        p.AccountLegalEntityId == command.AccountLegalEntity.Id &&
                        p.EmployerUserRef == actionedBy
                    ),
                    CancellationToken.None
                ), 
                Times.Once
            );

            _accountProviderLegalEntitiesWriteRepositoryMock.Verify(a => a.CreateAccountProviderLegalEntity(
               It.Is<AccountProviderLegalEntity>(p =>
                   p.AccountLegalEntityId == command.AccountLegalEntity.Id &&
                   p.Permissions.Count == request.PermissionRequests.Count &&
                   p.AccountProvider == accountProvider),
                   CancellationToken.None),
               Times.Once
            );
        }

        private void VerifyAccountCreation(AcceptCreateAccountRequestCommand command)
        {
            _accountWriteRepositoryMock.Verify(x =>
                x.CreateAccount(
                    It.Is<Account>(a =>
                        a.Id == command.Account.Id &&
                        a.Name == command.Account.Name
                    ),
                    CancellationToken.None
                ),
                Times.Once
            );
        }

        private void VerifyAccountLegalEntityCreation(AcceptCreateAccountRequestCommand command)
        {
            _accountLegalEntityWriteRepositoryMock.Verify(x => 
                x.CreateAccountLegalEntity(
                    It.Is<AccountLegalEntity>(a =>
                        a.Id == command.AccountLegalEntity.Id &&
                        a.AccountId == command.Account.Id &&
                        a.Name == command.AccountLegalEntity.Name
                    ),
                    CancellationToken.None
                ), 
                Times.Once
            );
        }

        private void VerifyAccountProviderCreation(long ukprn, long accountId)
        {
            _accountProviderWriteRepositoryMock.Verify(x => 
                x.CreateAccountProvider(
                    ukprn,
                    accountId,
                    CancellationToken.None
                ), 
                Times.Once
            );
        }

        private void VerifyProviderAddedEventPublished()
        {
            _messageSessionMock.Verify(m => 
                m.Publish(
                    It.IsAny<AddedAccountProviderEvent>(), 
                    It.IsAny<PublishOptions>(), 
                    It.IsAny<CancellationToken>()
                ), 
                Times.Once
            );
        }

        private void VerifyPermissionsUpdatedEventPublished()
        {
            _messageSessionMock.Verify(m => 
                m.Publish(
                    It.IsAny<UpdatedPermissionsEvent>(), 
                    It.IsAny<PublishOptions>(), 
                    It.IsAny<CancellationToken>()
                ), 
                Times.Once
            );
        }
    }
}
