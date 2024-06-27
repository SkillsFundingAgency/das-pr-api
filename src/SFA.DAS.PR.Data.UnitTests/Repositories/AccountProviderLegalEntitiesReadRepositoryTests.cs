using AutoFixture;
using NUnit.Framework;
using SFA.DAS.PR.Data.Repositories;
using SFA.DAS.PR.Data.UnitTests.InMemoryDatabases;
using SFA.DAS.PR.Data.UnitTests.Setup;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.PR.Data.UnitTests.Repositories;

public class AccountProviderLegalEntitiesReadRepositoryTests
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
    public async Task GetAccountProviderLegalEntities_Returns_Multiple_AccountProviderLegalEntities()
    {
        Account account = AccountTestData.CreateAccount(1004);

        List<AccountProviderLegalEntity> accountProviderLegalEntitiesToAdd =
            AccountProviderLegalEntityTestData.CreateAccountProviderLegalEntitiesWithPermissions(account);

        long ukprn = accountProviderLegalEntitiesToAdd.First().AccountProvider.ProviderUkprn;

        List<AccountProviderLegalEntity> result = new();

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(GetAccountProviderLegalEntities_Returns_Multiple_AccountProviderLegalEntities)}")
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

        Assert.Multiple(() =>
        {
            Assert.That(resultEntity, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(accountProvidersCount));
            Assert.That(resultEntity?.AccountProvider, Is.Not.Null);
            Assert.That(resultEntity?.AccountProvider.Account, Is.Not.Null);
            Assert.That(resultEntity?.AccountLegalEntity, Is.Not.Null);
            Assert.That(resultEntity?.Permissions, Has.Exactly(1).Items);
        });
    }

    [Test]
    public async Task GetAccountProviderLegalEntity_Returns_Entity()
    {
        Account account = AccountTestData.CreateAccount(1004);

        AccountProviderLegalEntity accountProviderLegalEntity =
            AccountProviderLegalEntityTestData.CreateAccountProviderLegalEntity(account);

        AccountProviderLegalEntity? result = new();

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(GetAccountProviderLegalEntity_Returns_Entity)}")
        )
        {
            await context.AddAsync(account);
            await context.AddAsync(accountProviderLegalEntity);
            await context.SaveChangesAsync(cancellationToken);

            AccountProviderLegalEntitiesReadRepository sut = new(context);

            result = await sut.GetAccountProviderLegalEntity(
                accountProviderLegalEntity.AccountProvider.ProviderUkprn,
                accountProviderLegalEntity.AccountLegalEntityId,
                cancellationToken
           );
        }

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result?.AccountProvider, Is.Not.Null);
            Assert.That(result?.Permissions, Has.Exactly(1).Items);
        });
    }
}