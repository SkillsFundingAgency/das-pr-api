using Moq;
using SFA.DAS.PR.Application.Requests.Commands.AcceptAddAccountRequest;
using SFA.DAS.PR.Data;
using SFA.DAS.PR.Data.UnitTests.Setup;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Application.UnitTests.Requests.Commands.AcceptAddAccountRequest;

[TestFixture]
public sealed class AcceptAddAccountRequestCommandHandlerTests
{
    private Mock<IProviderRelationshipsDataContext> _providerRelationshipsDataContextMock;
    private Mock<IRequestWriteRepository> _requestWriteRepositoryMock;
    private Mock<IAccountProviderLegalEntitiesWriteRepository> _accountProviderLegalEntitiesWriteRepositoryMock;
    private Mock<IAccountProviderWriteRepository> _accountProviderWriteRepositoryMock;
    private Mock<IMessageSession> _messageSessionMock;
    private Mock<IPermissionsAuditWriteRepository> _permissionsAuditWriteRepositoryMock;
    private Mock<IAccountLegalEntityReadRepository> _accountLegalEntityReadRepositoryMock;

    private AcceptAddAccountRequestCommandHandler _handler;

    [SetUp]
    public void CreateHandler()
    {
        _providerRelationshipsDataContextMock = new Mock<IProviderRelationshipsDataContext>();
        _accountProviderWriteRepositoryMock = new Mock<IAccountProviderWriteRepository>();
        _accountProviderLegalEntitiesWriteRepositoryMock = new Mock<IAccountProviderLegalEntitiesWriteRepository>();
        _permissionsAuditWriteRepositoryMock = new Mock<IPermissionsAuditWriteRepository>();
        _messageSessionMock = new Mock<IMessageSession>();
        _requestWriteRepositoryMock = new Mock<IRequestWriteRepository>();
        _accountLegalEntityReadRepositoryMock = new Mock<IAccountLegalEntityReadRepository>();

        _handler = new AcceptAddAccountRequestCommandHandler(
            _providerRelationshipsDataContextMock.Object,
            _requestWriteRepositoryMock.Object,
            _accountProviderLegalEntitiesWriteRepositoryMock.Object,
            _accountProviderWriteRepositoryMock.Object,
            _accountLegalEntityReadRepositoryMock.Object,
            _messageSessionMock.Object,
            _permissionsAuditWriteRepositoryMock.Object
        );
    }

    [Test]
    [MoqAutoData]
    public async Task AcceptAddAccountRequestCommandHandler_ShouldAcceptRequest(AcceptAddAccountRequestCommand command)
    {
        Guid actionedByGuid = Guid.NewGuid();
        command.ActionedBy = actionedByGuid.ToString();
        Request request = RequestTestData.Create(Guid.NewGuid());
        Account account = AccountTestData.CreateAccount(10001);

        AccountLegalEntity accountLegalEntity = AccountLegalEntityTestData.Create();
        AccountProvider accountProvider = AccountProviderTestData.CreateAccountProvider(1, account.Id, request.Ukprn);
        AccountProviderLegalEntity accountProviderLegalEntity = AccountProviderLegalEntityTestData.CreateAccountProviderLegalEntity(account);

        _requestWriteRepositoryMock.Setup(x =>
            x.GetRequest(
                It.IsAny<Guid>(),
                It.IsAny<CancellationToken>()
            )
        )
        .ReturnsAsync(request);

        _accountLegalEntityReadRepositoryMock.Setup(a =>
            a.GetAccountLegalEntity(
                request.AccountLegalEntityId!.Value,
                CancellationToken.None
            )
        )
        .ReturnsAsync(accountLegalEntity);

        _accountProviderWriteRepositoryMock.Setup(a =>
            a.GetAccountProvider(
                request.Ukprn,
                accountLegalEntity.AccountId,
                CancellationToken.None
            )
        )
        .ReturnsAsync(accountProvider);

        var result = await _handler.Handle(command, CancellationToken.None);

        _accountProviderLegalEntitiesWriteRepositoryMock.Verify(a => a.CreateAccountProviderLegalEntity(
               It.Is<AccountProviderLegalEntity>(p =>
                   p.AccountLegalEntityId == accountLegalEntity.Id &&
                   p.Permissions.Count == request.PermissionRequests.Count &&
                   p.AccountProvider == accountProvider),
                   CancellationToken.None),
               Times.Once
            );

        _permissionsAuditWriteRepositoryMock.Verify(a => a.RecordPermissionsAudit(
            It.Is<PermissionsAudit>(p =>
                p.Action == nameof(PermissionAction.AccountAdded) &&
                p.Ukprn == request.Ukprn &&
                p.AccountLegalEntityId == request.AccountLegalEntityId!.Value &&
                p.EmployerUserRef == actionedByGuid),
                CancellationToken.None),
            Times.Once
        );

        _providerRelationshipsDataContextMock.Verify(x =>
            x.SaveChangesAsync(
                CancellationToken.None
            ),
            Times.Exactly(1)
        );

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
