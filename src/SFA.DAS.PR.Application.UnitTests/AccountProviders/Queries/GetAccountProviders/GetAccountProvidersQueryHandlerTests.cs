using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Identity.Client;
using Moq;
using SFA.DAS.PR.Application.AccountProviders.Queries.GetAccountProviders;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Application.UnitTests.AccountProviders.Queries.GetAccountProviders
{
    public class GetAccountProvidersQueryHandlerTests
    {
        [Test]
        [RecursiveMoqAutoData]
        public async Task Handle_ProvidersFound_ReturnsAccountProvidersResult(
            [Frozen] Mock<IAccountLegalEntityReadRepository> accountLegalEntityReadRepository,
            GetAccountProvidersQueryHandler sut,
            List<AccountLegalEntity> legalEntities,
            long accountId,
            CancellationToken cancellationToken
        )
        {
            accountLegalEntityReadRepository.Setup(a =>
                a.GetAccountLegalEntities(accountId, cancellationToken)
            ).ReturnsAsync(legalEntities);

            GetAccountProvidersQueryResult expectedResult = new(accountId, AccountProviderModel.BuildAccountProviderModels(legalEntities));

            ValidatedResponse<GetAccountProvidersQueryResult> result =
                await sut.Handle(new GetAccountProvidersQuery(accountId), cancellationToken);

            result.Result.Should().BeEquivalentTo(expectedResult, c => c.ExcludingMissingMembers());
        }

        [Test]
        [RecursiveMoqAutoData]
        public async Task Handle_ProvidersNotFound_ReturnsEmptyResult(
            [Frozen] Mock<IAccountLegalEntityReadRepository> accountLegalEntityReadRepository,
            GetAccountProvidersQueryHandler sut,
            long accountId,
            CancellationToken cancellationToken
        )
        {
            accountLegalEntityReadRepository.Setup(a =>
                a.GetAccountLegalEntities(accountId, cancellationToken)
            ).ReturnsAsync(() => new List<AccountLegalEntity>());

            GetAccountProvidersQueryResult expectedResult = new(accountId, []);

            var result = await sut.Handle(new GetAccountProvidersQuery(accountId), cancellationToken);

            result.Result.Should().BeEquivalentTo(expectedResult, c => c.ExcludingMissingMembers());
        }
    }
}
