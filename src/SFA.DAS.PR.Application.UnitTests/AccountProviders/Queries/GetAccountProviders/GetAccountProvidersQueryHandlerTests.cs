using AutoFixture.NUnit3;
using FluentAssertions;
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
            [Frozen] Mock<IAccountProvidersReadRepository> accountProvidersReadRepository,
            GetAccountProvidersQueryHandler sut,
            List<AccountProvider> providers,
            string accountPublicHashId,
            CancellationToken cancellationToken
        )
        {
            accountProvidersReadRepository.Setup(a =>
                a.GetAccountProviders(accountPublicHashId, cancellationToken)
            ).ReturnsAsync(providers);

            GetAccountProvidersQueryResult expectedResult = new(accountPublicHashId, providers.Select(a => (AccountProviderModel)a).ToList());

            ValidatedResponse<GetAccountProvidersQueryResult> result =
                await sut.Handle(new GetAccountProvidersQuery(accountPublicHashId), cancellationToken);

            result.Result.Should().BeEquivalentTo(expectedResult, c => c.ExcludingMissingMembers());
        }

        [Test]
        [RecursiveMoqAutoData]
        public async Task Handle_ProvidersNotFound_ReturnsEmptyResult(
            [Frozen] Mock<IAccountProvidersReadRepository> accountProvidersReadRepository,
            GetAccountProvidersQueryHandler sut,
            string accountPublicHashId,
            CancellationToken cancellationToken
        )
        {
            accountProvidersReadRepository.Setup(a =>
                a.GetAccountProviders(accountPublicHashId, cancellationToken)
            ).ReturnsAsync(() => new List<AccountProvider>());

            GetAccountProvidersQueryResult expectedResult = new(accountPublicHashId, []);

            var result = await sut.Handle(new GetAccountProvidersQuery(accountPublicHashId), cancellationToken);

            result.Result.Should().BeEquivalentTo(expectedResult, c => c.ExcludingMissingMembers());
        }
    }
}
