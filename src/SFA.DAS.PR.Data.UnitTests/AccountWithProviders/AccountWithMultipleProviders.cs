using AutoFixture;
using NUnit.Framework;
using SFA.DAS.PR.Data.Repositories;
using SFA.DAS.PR.Data.UnitTests.InMemoryDatabases;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Data.UnitTests.AccountWithProviders
{
    public class AccountWithMultipleProviders
    {
        private CancellationToken cancellationToken = CancellationToken.None;

        private Fixture _fixture = null!;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Test]
        public async Task Multiple_AccountProviders_Are_Returned()
        {
            List<Account> accountsToAdd = new()
            {
                new()
                {
                    Id = 1001,
                    HashedId = Guid.NewGuid().ToString(),
                    PublicHashedId = Guid.NewGuid().ToString(),
                    Name = "AccountName",
                    Created = DateTime.UtcNow.AddDays(-1),
                    Updated = DateTime.UtcNow,
                    AccountProviders = new()
                    {
                        new()
                        {
                            Id = 1002,
                            AccountId = 1001,
                            ProviderUkprn = 1006,
                            Created = DateTime.UtcNow.AddDays(-1),
                            Provider = new()
                            {
                                Ukprn = 1006,
                                Name = "ProviderName",
                                Created = DateTime.UtcNow.AddDays(-1),
                                Updated = DateTime.UtcNow
                            },
                            AccountProviderLegalEntities = new()
                            {
                                new()
                                {
                                    Id = 1005,
                                    AccountProviderId = 1002,
                                    AccountLegalEntityId = 1004,
                                    Created = DateTime.UtcNow.AddDays(-1),
                                    Updated = DateTime.UtcNow,
                                    Permissions = new()
                                    {
                                        new()
                                        {
                                            AccountProviderLegalEntityId = 1005,
                                            Id = 106,
                                            Operation = Operation.CreateCohort
                                        }
                                    },
                                }
                            }
                        }
                    },
                    AccountLegalEntities = new()
                    {
                        new()
                        {
                            Id = 1004,
                            PublicHashedId = Guid.NewGuid().ToString(),
                            AccountId = 1001,
                            Name = "AccountLegalEntityName",
                            Created = DateTime.UtcNow.AddDays(-1),
                            Updated = DateTime.UtcNow,
                            Deleted = null
                        }
                    }
                }
            };

            string accountHashedId = accountsToAdd.First().PublicHashedId;

            List<AccountProvider> result = new();

            using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
                $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(Multiple_AccountProviders_Are_Returned)}")
            )
            {
                await context.AddRangeAsync(accountsToAdd);
                await context.SaveChangesAsync(cancellationToken);

                AccountProvidersReadRepository sut = new(context);

                result = await sut.GetAccountProviders(accountHashedId, cancellationToken);
            }

            var accountProvidersCount = accountsToAdd.First().AccountProviders.Count();

            var resultEntity = result.FirstOrDefault();

            Assert.That(resultEntity, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(accountProvidersCount));
            Assert.That(resultEntity.Provider, Is.Not.Null);
            Assert.That(resultEntity.AccountProviderLegalEntities, Has.Exactly(1).Items);
            Assert.That(resultEntity.AccountProviderLegalEntities.First().AccountLegalEntity, Is.Not.Null);
            Assert.That(resultEntity.AccountProviderLegalEntities.First().Permissions, Has.Exactly(1).Items);
        }
    }
}