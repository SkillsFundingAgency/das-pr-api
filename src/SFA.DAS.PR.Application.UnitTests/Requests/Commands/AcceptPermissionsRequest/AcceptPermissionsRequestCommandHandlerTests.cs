﻿using Moq;
using SFA.DAS.PR.Application.Requests.Commands.AcceptPermissionsRequest;
using SFA.DAS.PR.Data;
using SFA.DAS.PR.Data.UnitTests.Setup;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.ProviderRelationships.Messages.Events;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Application.UnitTests.Requests.Commands.AcceptPermissionsRequest;

[TestFixture]
public sealed class AcceptPermissionsRequestCommandHandlerTests
{
    private Mock<IProviderRelationshipsDataContext> _providerRelationshipsDataContextMock;
    private Mock<IRequestWriteRepository> _requestWriteRepositoryMock;
    private Mock<IAccountProviderLegalEntitiesReadRepository> _accountProviderLegalEntitiesReadRepositoryMock;
    private Mock<IAccountProviderWriteRepository> _accountProviderWriteRepositoryMock;
    private Mock<IPermissionsWriteRepository> _permissionsWriteRepositoryMock;
    private Mock<IMessageSession> _messageSessionMock;
    private Mock<IPermissionsAuditWriteRepository> _permissionsAuditWriteRepositoryMock;
    private Mock<IAccountLegalEntityReadRepository> _accountLegalEntityReadRepositoryMock;

    private AcceptPermissionsRequestCommandHandler _handler;

    [SetUp]
    public void CreateHandler()
    {
        _providerRelationshipsDataContextMock = new Mock<IProviderRelationshipsDataContext>();
        _accountProviderWriteRepositoryMock = new Mock<IAccountProviderWriteRepository>();
        _accountProviderLegalEntitiesReadRepositoryMock = new Mock<IAccountProviderLegalEntitiesReadRepository>();
        _permissionsWriteRepositoryMock = new Mock<IPermissionsWriteRepository>();
        _permissionsAuditWriteRepositoryMock = new Mock<IPermissionsAuditWriteRepository>();
        _messageSessionMock = new Mock<IMessageSession>();
        _requestWriteRepositoryMock = new Mock<IRequestWriteRepository>();
        _accountLegalEntityReadRepositoryMock = new Mock<IAccountLegalEntityReadRepository>();

        _handler = new AcceptPermissionsRequestCommandHandler(
            _providerRelationshipsDataContextMock.Object,
            _requestWriteRepositoryMock.Object,
            _accountProviderLegalEntitiesReadRepositoryMock.Object,
            _accountLegalEntityReadRepositoryMock.Object,
            _permissionsWriteRepositoryMock.Object,
            _messageSessionMock.Object,
            _permissionsAuditWriteRepositoryMock.Object
        );
    }

    [Test]
    [MoqAutoData]
    public async Task AcceptPermissionsRequestCommandHandler_ShouldAcceptRequest(AcceptPermissionsRequestCommand command)
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

        _accountProviderLegalEntitiesReadRepositoryMock.Setup(a => 
            a.GetAccountProviderLegalEntity(
                request.Ukprn,
                accountLegalEntity.Id, 
                CancellationToken.None
            )
        )
        .ReturnsAsync(accountProviderLegalEntity);

        var result = await _handler.Handle(command, CancellationToken.None);

        _permissionsWriteRepositoryMock.Verify(x => 
            x.CreatePermissions(
                It.IsAny<IEnumerable<Permission>>()
            ), 
            Times.Once
        );

        _permissionsAuditWriteRepositoryMock.Verify(a => a.RecordPermissionsAudit(
            It.Is<PermissionsAudit>(p =>
                p.Action == nameof(PermissionAction.PermissionUpdated) &&
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
