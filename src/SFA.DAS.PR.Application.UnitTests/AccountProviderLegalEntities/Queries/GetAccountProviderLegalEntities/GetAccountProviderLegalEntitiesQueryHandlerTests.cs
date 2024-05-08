﻿using AutoFixture.NUnit3;
using Moq;
using SFA.DAS.PR.Application.AccountProviderLegalEntities.Queries.GetAccountProviderLegalEntities;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using FluentAssertions;

namespace SFA.DAS.PR.Application.UnitTests.AccountProviderLegalEntities.Queries.GetAccountProviderLegalEntities;
public class GetAccountProviderLegalEntitiesQueryHandlerTests
{
    [Test]
    [RecursiveMoqAutoData]
    public async Task Handle_AccountProviderLegalEntitiesFound_ReturnsAccountProviderLegalEntitiesResult(
            [Frozen] Mock<IAccountProviderLegalEntitiesReadRepository> accountProviderLegalEntitiesReadRepository,
            GetAccountProviderLegalEntitiesQueryHandler sut,
            List<AccountProviderLegalEntity> entities,
            CancellationToken cancellationToken
        )
    {
        GetAccountProviderLegalEntitiesQuery query = new()
        {
            Ukprn = null,
            AccountHashedId = "Hash",
            Operations = [Operation.Recruitment]
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
        CancellationToken cancellationToken
    )
    {
        GetAccountProviderLegalEntitiesQuery query = new()
        {
            Ukprn = null,
            AccountHashedId = "Hash",
            Operations = [Operation.Recruitment]
        };

        accountProviderLegalEntitiesReadRepository.Setup(a =>
            a.GetAccountProviderLegalEntities(query.Ukprn, query.AccountHashedId, query.Operations, cancellationToken)
        ).ReturnsAsync(new List<AccountProviderLegalEntity>());

        GetAccountProviderLegalEntitiesQueryResult expectedResult = new([]);

        var result = await sut.Handle(query, cancellationToken);

        result.Result.Should().BeEquivalentTo(expectedResult, c => c.ExcludingMissingMembers());
    }
}
