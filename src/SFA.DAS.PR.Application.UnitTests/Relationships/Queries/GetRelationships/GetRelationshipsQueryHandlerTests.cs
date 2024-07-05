using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Application.Relationships.Queries.GetRelationships;
using SFA.DAS.PR.Data.UnitTests.Setup;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Application.UnitTests.Relationships.Queries.GetRelationships;

public class GetRelationshipsQueryHandlerTests
{
    private readonly CancellationToken cancellationToken = CancellationToken.None;

    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_GetRelationships_Returns_GetProviderEmployerRelationshipQueryResult(
        [Frozen] Mock<IAccountProviderLegalEntitiesReadRepository> accountProviderLegalEntitiesReadRepository,
        [Frozen] Mock<IPermissionAuditReadRepository> permissionAuditReadRepository,
        [Frozen] Mock<IRequestReadRepository> requestReadRepository,
        GetRelationshipsQueryHandler sut,
        AccountProviderLegalEntity accountProviderLegalEntity,
        GetRelationshipsQuery query
    )
    {
        PermissionsAudit? audit = PermissionsAuditTestData.Create(Guid.NewGuid());

        Request? request = RequestTestData.Create(Guid.NewGuid());

        accountProviderLegalEntitiesReadRepository.Setup(a =>
            a.GetAccountProviderLegalEntityByProvider(
                query.Ukprn!.Value,
                query.AccountLegalEntityId!.Value,
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(accountProviderLegalEntity);

        permissionAuditReadRepository.Setup(a =>
            a.GetMostRecentPermissionAudit(
                query.Ukprn!.Value,
                query.AccountLegalEntityId!.Value,
                cancellationToken
            )
        ).ReturnsAsync(audit);

        requestReadRepository.Setup(a =>
            a.GetRequest(
                query.Ukprn!.Value,
                query.AccountLegalEntityId!.Value,
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(request);

        ValidatedResponse<GetRelationshipsQueryResult?> result = await sut.Handle(query, cancellationToken);

        result.Result.Should().BeOfType<GetRelationshipsQueryResult>();

        Assert.Multiple(() =>
        {
            Assert.That(result.Result, Is.Not.Null, "Result should not be null.");
            Assert.That(result.Result!.AccountLegalEntityId, Is.EqualTo(accountProviderLegalEntity.AccountLegalEntityId), $"{result.Result!.AccountLegalEntityId} should be equal to {accountProviderLegalEntity.AccountLegalEntityId}.");
            Assert.That(result.Result!.AccountLegalEntitypublicHashedId, Is.EqualTo(accountProviderLegalEntity.AccountLegalEntity.PublicHashedId), $"{result.Result!.AccountLegalEntitypublicHashedId} should be equal to {accountProviderLegalEntity.AccountLegalEntity.PublicHashedId}.");
            Assert.That(result.Result!.AccountLegalEntityName, Is.EqualTo(accountProviderLegalEntity.AccountLegalEntity.Name), $"{result.Result!.AccountLegalEntityName} should be equal to {accountProviderLegalEntity.AccountLegalEntity.Name}.");
            Assert.That(result.Result!.AccountId, Is.EqualTo(accountProviderLegalEntity.AccountLegalEntity.AccountId), $"{result.Result!.AccountId} should be equal to {accountProviderLegalEntity.AccountLegalEntity.AccountId}.");
            Assert.That(result.Result!.Ukprn, Is.EqualTo(accountProviderLegalEntity.AccountProvider.ProviderUkprn), $"{result.Result!.Ukprn} should be equal to {accountProviderLegalEntity.AccountProvider.ProviderUkprn}.");
            Assert.That(result.Result!.ProviderName, Is.EqualTo(accountProviderLegalEntity.AccountProvider.Provider.Name), $"{result.Result!.ProviderName} should be equal to {accountProviderLegalEntity.AccountProvider.Provider.Name}.");
            Assert.That(result.Result!.Operations, Is.EquivalentTo(accountProviderLegalEntity.Permissions.Select(a => a.Operation).ToArray()));

            Assert.That(result.Result!.LastAction, Is.EqualTo(Enum.Parse<PermissionAction>(audit.Action)));
            Assert.That(result.Result!.LastActionTime, Is.EqualTo(audit.Eventtime));

            Assert.That(result.Result!.LastRequestType, Is.EqualTo(request!.RequestType));
            Assert.That(result.Result!.LastRequestTime, Is.EqualTo(request!.UpdatedDate));
            Assert.That(result.Result!.LastRequestStatus, Is.EqualTo(Enum.Parse<RequestStatus>(request.Status!)));
            Assert.That(result.Result!.LastRequestOperations, Is.EquivalentTo(request.PermissionRequests.Select(a => (Operation)a.Operation).ToArray()));
        });
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_GetRelationships_Null_Permissions_Returns_GetProviderEmployerRelationshipQueryResult(
        [Frozen] Mock<IAccountProviderLegalEntitiesReadRepository> accountProviderLegalEntitiesReadRepository,
        [Frozen] Mock<IPermissionAuditReadRepository> permissionAuditReadRepository,
        [Frozen] Mock<IRequestReadRepository> requestReadRepository,
        GetRelationshipsQueryHandler sut,
        AccountProviderLegalEntity accountProviderLegalEntity,
        GetRelationshipsQuery query
    )
    {
        PermissionsAudit? audit = null;

        Request? request = null;

        accountProviderLegalEntitiesReadRepository.Setup(a =>
            a.GetAccountProviderLegalEntityByProvider(
                query.Ukprn!.Value,
                query.AccountLegalEntityId!.Value,
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(accountProviderLegalEntity);

        permissionAuditReadRepository.Setup(a =>
            a.GetMostRecentPermissionAudit(
                query.Ukprn!.Value,
                query.AccountLegalEntityId!.Value,
                cancellationToken
            )
        ).ReturnsAsync(audit);

        requestReadRepository.Setup(a =>
            a.GetRequest(
                query.Ukprn!.Value,
                query.AccountLegalEntityId!.Value,
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(request);

        ValidatedResponse<GetRelationshipsQueryResult?> result = await sut.Handle(query, cancellationToken);

        result.Result.Should().BeOfType<GetRelationshipsQueryResult>();

        Assert.Multiple(() =>
        {
            Assert.That(result.Result, Is.Not.Null, "Result should not be null.");
            Assert.That(result.Result!.AccountLegalEntityId, Is.EqualTo(accountProviderLegalEntity.AccountLegalEntityId), $"{result.Result!.AccountLegalEntityId} should be equal to {accountProviderLegalEntity.AccountLegalEntityId}.");
            Assert.That(result.Result!.AccountLegalEntitypublicHashedId, Is.EqualTo(accountProviderLegalEntity.AccountLegalEntity.PublicHashedId), $"{result.Result!.AccountLegalEntitypublicHashedId} should be equal to {accountProviderLegalEntity.AccountLegalEntity.PublicHashedId}.");
            Assert.That(result.Result!.AccountLegalEntityName, Is.EqualTo(accountProviderLegalEntity.AccountLegalEntity.Name), $"{result.Result!.AccountLegalEntityName} should be equal to {accountProviderLegalEntity.AccountLegalEntity.Name}.");
            Assert.That(result.Result!.AccountId, Is.EqualTo(accountProviderLegalEntity.AccountLegalEntity.AccountId), $"{result.Result!.AccountId} should be equal to {accountProviderLegalEntity.AccountLegalEntity.AccountId}.");
            Assert.That(result.Result!.Ukprn, Is.EqualTo(accountProviderLegalEntity.AccountProvider.ProviderUkprn), $"{result.Result!.Ukprn} should be equal to {accountProviderLegalEntity.AccountProvider.ProviderUkprn}.");
            Assert.That(result.Result!.ProviderName, Is.EqualTo(accountProviderLegalEntity.AccountProvider.Provider.Name), $"{result.Result!.ProviderName} should be equal to {accountProviderLegalEntity.AccountProvider.Provider.Name}.");
            Assert.That(result.Result!.Operations, Is.EquivalentTo(accountProviderLegalEntity.Permissions.Select(a => a.Operation).ToArray()));

            Assert.That(result.Result!.LastAction, Is.Null);
            Assert.That(result.Result!.LastActionTime, Is.EqualTo(accountProviderLegalEntity.Updated));

            Assert.That(result.Result!.LastRequestType, Is.Null);
            Assert.That(result.Result!.LastRequestTime, Is.Null);
            Assert.That(result.Result!.LastRequestStatus, Is.Null);
            Assert.That(result.Result!.LastRequestOperations, Is.Null);
        });
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_GetRelationships_Null_AccountProviderLegalEntity_Returns_Null_GetProviderEmployerRelationshipQueryResult(
        [Frozen] Mock<IAccountProviderLegalEntitiesReadRepository> accountProviderLegalEntitiesReadRepository,
        [Frozen] Mock<IPermissionAuditReadRepository> permissionAuditReadRepository,
        [Frozen] Mock<IRequestReadRepository> requestReadRepository,
        GetRelationshipsQueryHandler sut,
        GetRelationshipsQuery query
    )
    {
        AccountProviderLegalEntity? accountProviderLegalEntity = null;

        accountProviderLegalEntitiesReadRepository.Setup(a =>
            a.GetAccountProviderLegalEntityByProvider(
                query.Ukprn!.Value,
                query.AccountLegalEntityId!.Value,
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(accountProviderLegalEntity);

        ValidatedResponse<GetRelationshipsQueryResult?> result = await sut.Handle(query, cancellationToken);

        result.As<ValidatedResponse<GetRelationshipsQueryResult?>>().IsValidResponse.Should().BeTrue();
        result.Result.Should().BeNull();
    }
}