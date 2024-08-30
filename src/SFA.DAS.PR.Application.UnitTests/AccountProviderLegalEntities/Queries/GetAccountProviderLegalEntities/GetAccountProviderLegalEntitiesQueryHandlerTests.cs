using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.PR.Application.AccountProviderLegalEntities.Queries.GetAccountProviderLegalEntities;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Application.UnitTests.AccountProviderLegalEntities.Queries.GetAccountProviderLegalEntities;
public class GetAccountProviderLegalEntitiesQueryHandlerTests
{
    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_AccountProviderLegalEntitiesFound_ReturnsAccountProviderLegalEntitiesResult(
        [Frozen] Mock<IAccountProviderLegalEntitiesReadRepository> accountProviderLegalEntitiesReadRepository,
        GetAccountProviderLegalEntitiesQueryHandler sut,
        List<AccountProviderLegalEntity> entities,
        CancellationToken cancellationToken)
    {
        GetAccountProviderLegalEntitiesQuery query = new()
        {
            Ukprn = null,
            AccountHashedId = "Hash",
            Operations = [Operation.CreateCohort]
        };

        accountProviderLegalEntitiesReadRepository.Setup(a =>
            a.GetAccountProviderLegalEntities(query.Ukprn, query.AccountHashedId, query.Operations, cancellationToken)
        ).ReturnsAsync(entities);

        GetAccountProviderLegalEntitiesQueryResult expectedResult = new(entities.Select(a => (AccountProviderLegalEntityModel)a).ToList());

        ValidatedResponse<GetAccountProviderLegalEntitiesQueryResult> result = await sut.Handle(query, cancellationToken);

        result.Result.Should().BeEquivalentTo(expectedResult, c => c.ExcludingMissingMembers());
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_AccountProviderLegalEntitiesNotFound_ReturnsEmptyResult(
        [Frozen] Mock<IAccountProviderLegalEntitiesReadRepository> accountProviderLegalEntitiesReadRepository,
        GetAccountProviderLegalEntitiesQueryHandler sut,
        CancellationToken cancellationToken)
    {
        GetAccountProviderLegalEntitiesQuery query = new()
        {
            Ukprn = null,
            AccountHashedId = "Hash",
            Operations = [Operation.CreateCohort]
        };

        accountProviderLegalEntitiesReadRepository.Setup(a =>
            a.GetAccountProviderLegalEntities(query.Ukprn, query.AccountHashedId, query.Operations, cancellationToken)
        ).ReturnsAsync(new List<AccountProviderLegalEntity>());

        GetAccountProviderLegalEntitiesQueryResult expectedResult = new([]);

        var result = await sut.Handle(query, cancellationToken);

        result.Result.Should().BeEquivalentTo(expectedResult, c => c.ExcludingMissingMembers());
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_OperationRecruitment_IncludesRecruitmentRequiresReview(
        [Frozen] Mock<IAccountProviderLegalEntitiesReadRepository> accountProviderLegalEntitiesReadRepository,
        GetAccountProviderLegalEntitiesQueryHandler sut,
        List<AccountProviderLegalEntity> entities,
        CancellationToken cancellationToken)
    {
        GetAccountProviderLegalEntitiesQuery query = new()
        {
            Ukprn = null,
            AccountHashedId = "Hash",
            Operations = [Operation.Recruitment]
        };

        List<Operation> expectedOperations = [Operation.Recruitment, Operation.RecruitmentRequiresReview];

        await sut.Handle(query, cancellationToken);

        accountProviderLegalEntitiesReadRepository.Verify(a => a.GetAccountProviderLegalEntities(query.Ukprn, query.AccountHashedId, expectedOperations, cancellationToken));
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_OperationRecruitmentRequiresReview_IncludesRecruitmentRequiresReviewOnly(
        [Frozen] Mock<IAccountProviderLegalEntitiesReadRepository> accountProviderLegalEntitiesReadRepository,
        GetAccountProviderLegalEntitiesQueryHandler sut,
        List<AccountProviderLegalEntity> entities,
        CancellationToken cancellationToken)
    {
        GetAccountProviderLegalEntitiesQuery query = new()
        {
            Ukprn = null,
            AccountHashedId = "Hash",
            Operations = [Operation.RecruitmentRequiresReview]
        };

        List<Operation> expectedOperations = [Operation.RecruitmentRequiresReview];

        await sut.Handle(query, cancellationToken);

        accountProviderLegalEntitiesReadRepository.Verify(a => a.GetAccountProviderLegalEntities(query.Ukprn, query.AccountHashedId, expectedOperations, cancellationToken));
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_AllRecruitment(
        [Frozen] Mock<IAccountProviderLegalEntitiesReadRepository> accountProviderLegalEntitiesReadRepository,
        GetAccountProviderLegalEntitiesQueryHandler sut,
        List<AccountProviderLegalEntity> entities,
        CancellationToken cancellationToken)
    {
        GetAccountProviderLegalEntitiesQuery query = new()
        {
            Ukprn = null,
            AccountHashedId = "Hash",
            Operations = [Operation.Recruitment, Operation.RecruitmentRequiresReview]
        };

        List<Operation> expectedOperations = [Operation.Recruitment, Operation.RecruitmentRequiresReview];

        await sut.Handle(query, cancellationToken);

        accountProviderLegalEntitiesReadRepository.Verify(a => a.GetAccountProviderLegalEntities(query.Ukprn, query.AccountHashedId, expectedOperations, cancellationToken));
    }
}
