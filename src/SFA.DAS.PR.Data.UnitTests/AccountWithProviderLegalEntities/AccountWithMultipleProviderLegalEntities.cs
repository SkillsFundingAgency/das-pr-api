using AutoFixture;
using NUnit.Framework;
using SFA.DAS.PR.Data.Repositories;
using SFA.DAS.PR.Data.UnitTests.InMemoryDatabases;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Data.UnitTests.AccountWithProviderLegalEntities;

public class AccountWithMultipleProviderLegalEntities
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
    public async Task Multiple_AccountProviderLegalEntities_Are_Returned()
    {
        Account account = new()
        {
            Id = 1004,
            HashedId = "hash",
            PublicHashedId = "phash",
            Name = "Name",
            Created = DateTime.Now.AddDays(-1),
            Updated = DateTime.Now
        };

        List<AccountProviderLegalEntity> accountProviderLegalEntitiesToAdd = new()
        {
            new()
            {
                Id = 1001,
                AccountProviderId = 1002,
                AccountLegalEntityId = 1003,
                Created = DateTime.UtcNow.AddDays(-1),
                Updated = DateTime.UtcNow,
                Permissions = new List<Permission>()
                {
                    new ()
                    {
                        Id = 1,
                        AccountProviderLegalEntityId = 1001,
                        Operation = Operation.Recruitment
                    }
                },
                AccountLegalEntity = new()
                {
                    Id = 1003,
                    PublicHashedId = "hash",
                    AccountId = 1004,
                    Account = account,
                    Name = "Name",
                    Created = DateTime.Now.AddDays(-1),
                    Updated = DateTime.Now,
                    Deleted = null
                },
                AccountProvider = new()
                {
                    Id = 1002,
                    AccountId = 1004,
                    Account = account,
                    ProviderUkprn = 1005,
                    Created = DateTime.Now.AddDays(-1),
                    Provider = new()
                    {
                        Ukprn = 1005,
                        Name = "Name",
                        Created = DateTime.Now.AddDays(-1),
                        Updated = DateTime.Now
                    }
                }
            }
        };

        long ukprn = accountProviderLegalEntitiesToAdd.First().AccountProvider.ProviderUkprn;

        List<AccountProviderLegalEntity> result = new();

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(Multiple_AccountProviderLegalEntities_Are_Returned)}")
        )
        {
            await context.AddAsync(account);
            await context.AddRangeAsync(accountProviderLegalEntitiesToAdd);
            await context.SaveChangesAsync(cancellationToken);

            AccountProviderLegalEntitiesReadRepository sut = new(context);

            result = await sut.GetAccountProviderLegalEntities(ukprn, null, [Operation.Recruitment], cancellationToken);
        }

        var accountProvidersCount = accountProviderLegalEntitiesToAdd.Count();

        var resultEntity = result.FirstOrDefault();

        Assert.That(resultEntity, Is.Not.Null);
        Assert.That(result, Has.Count.EqualTo(accountProvidersCount));
        Assert.That(resultEntity.AccountProvider, Is.Not.Null);
        Assert.That(resultEntity.AccountProvider.Account, Is.Not.Null);
        Assert.That(resultEntity.AccountLegalEntity, Is.Not.Null);
        Assert.That(resultEntity.Permissions, Has.Exactly(1).Items);
    }
}
