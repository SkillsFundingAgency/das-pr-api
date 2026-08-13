using AutoFixture.NUnit4;
using FluentAssertions;
using Moq;
using SFA.DAS.PR.Application.AccountProviders.Queries.GetAccountProviders;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Common;
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
            long accountId,
            long ukprn,
            CancellationToken cancellationToken
        )
        {
            var legalEntities = new List<AccountLegalEntity>()
            {
                new AccountLegalEntity
                {
                    Id = 1,
                    PublicHashedId = "ABC123",
                    AccountId = accountId,
                    Name = "Test Legal Entity",
                    Account = new Account
                    {
                        Id = accountId,
                        Name = "Test Account",
                        AccountProviders = new List<AccountProvider>
                        {
                            new AccountProvider
                            {
                                Id = 1,
                                AccountId = accountId,
                                ProviderUkprn = ukprn,
                                Provider = new Provider
                                {
                                    Ukprn = ukprn,
                                    Name = "Test Provider",
                                    Status = null
                                },
                            }
                        }
                    },
                }
            };

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

        [Test]
        [RecursiveMoqAutoData]
        public async Task Handle_ProvidersIsRemoved_ReturnsEmptyResultt(
            [Frozen] Mock<IAccountLegalEntityReadRepository> accountLegalEntityReadRepository,
            GetAccountProvidersQueryHandler sut,
            long accountId,
            long ukprn,
            CancellationToken cancellationToken
        )
        {
            var legalEntities = new List<AccountLegalEntity>()
            {
                new AccountLegalEntity
                {
                    Id = 1,
                    PublicHashedId = "ABC123",
                    AccountId = accountId,
                    Name = "Test Legal Entity",
                    Account = new Account
                    {
                        Id = accountId,
                        Name = "Test Account",
                        AccountProviders = new List<AccountProvider>
                        {
                            new AccountProvider
                            {
                                Id = 1,
                                AccountId = accountId,
                                ProviderUkprn = ukprn,
                                Provider = new Provider
                                {
                                    Ukprn = ukprn,
                                    Name = "Test Provider",
                                    Status = ProviderStatus.Removed
                                },
                            }
                        }
                    },
                }
            };
            accountLegalEntityReadRepository.Setup(a =>
                a.GetAccountLegalEntities(accountId, cancellationToken)
            ).ReturnsAsync(legalEntities);

            GetAccountProvidersQueryResult expectedResult = new(accountId, []);

            var result = await sut.Handle(new GetAccountProvidersQuery(accountId), cancellationToken);

            result.Result.Should().BeEquivalentTo(expectedResult, c => c.ExcludingMissingMembers());
        }
    }
}
