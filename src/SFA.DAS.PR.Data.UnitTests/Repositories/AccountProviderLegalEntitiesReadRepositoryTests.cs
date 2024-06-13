using NUnit.Framework;
using SFA.DAS.PR.Data.Repositories;
using SFA.DAS.PR.Data.UnitTests.InMemoryDatabases;
using SFA.DAS.PR.Data.UnitTests.Setup;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Data.UnitTests.Repositories;

public class AccountProviderLegalEntitiesReadRepositoryTests
{
    private CancellationToken cancellationToken = CancellationToken.None;

    [Test]
    public async Task GetAccountProviderLegalEntities_Returns_Success()
    {
        Account account = AccountTestData.CreateAccount(1004);

        List<AccountProviderLegalEntity> accountProviderLegalEntitiesToAdd =
            AccountProviderLegalEntityTestData.CreateAccountProviderLegalEntitiesWithPermissions(account);

        long ukprn = accountProviderLegalEntitiesToAdd.First().AccountProvider.ProviderUkprn;

        List<AccountProviderLegalEntity> result = new();

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(GetAccountProviderLegalEntities_Returns_Success)}")
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

    [Test]
    public async Task GetAccountProviderLegalEntityByProvider_Returns_Success()
    {
        Account account = AccountTestData.CreateAccount(1004);

        AccountProviderLegalEntity accountProviderLegalEntity =
            AccountProviderLegalEntityTestData.CreateAccountProviderLegalEntitiesWithPermissions(account).First();

        long ukprn = accountProviderLegalEntity.AccountProvider.ProviderUkprn;

        AccountProviderLegalEntity? result = null;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(GetAccountProviderLegalEntityByProvider_Returns_Success)}")
        )
        {
            await context.AddAsync(account);
            await context.AddAsync(accountProviderLegalEntity);
            await context.SaveChangesAsync(cancellationToken);

            AccountProviderLegalEntitiesReadRepository sut = new(context);

            result = await sut.GetAccountProviderLegalEntityByProvider(ukprn, accountProviderLegalEntity.AccountLegalEntityId, cancellationToken);
        }

        Assert.That(result, Is.Not.Null);
        Assert.That(result.AccountProvider, Is.Not.Null);
        Assert.That(result.AccountProvider.Account, Is.Not.Null);
        Assert.That(result.AccountLegalEntity, Is.Not.Null);
        Assert.That(result.Permissions, Has.Exactly(1).Items);
    }
}