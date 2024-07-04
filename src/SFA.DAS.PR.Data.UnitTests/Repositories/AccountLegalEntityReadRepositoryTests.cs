using AutoFixture;
using NUnit.Framework;
using SFA.DAS.PR.Data.Repositories;
using SFA.DAS.PR.Data.UnitTests.InMemoryDatabases;
using SFA.DAS.PR.Data.UnitTests.Setup;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Data.UnitTests.Repositories;

public class AccountLegalEntityReadRepositoryTests
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
    public async Task GetAccountLegalEntiies_Returns_List()
    {
        List<Account> accountsToAdd = AccountTestData.CreateAccounts();

        long accountId = accountsToAdd.First().Id;

        List<AccountLegalEntity> result = new();

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(GetAccountLegalEntiies_Returns_List)}")
        )
        {
            await context.AddRangeAsync(accountsToAdd);
            await context.SaveChangesAsync(cancellationToken);

            AccountLegalEntityReadRepository sut = new(context);

            result = await sut.GetAccountLegalEntities(accountId, cancellationToken);
        }

        var accountProvidersCount = accountsToAdd.First().AccountProviders.Count;

        var resultEntity = result.FirstOrDefault();

        Assert.Multiple(() =>
        {
            Assert.That(resultEntity, Is.Not.Null);
            Assert.That(result, Has.Count.EqualTo(accountProvidersCount));
            Assert.That(resultEntity?.Account, Is.Not.Null);
            Assert.That(resultEntity?.Account.AccountProviders, Has.Exactly(1).Items);
            Assert.That(resultEntity?.AccountProviderLegalEntities, Has.Exactly(1).Items);
            Assert.That(resultEntity?.AccountProviderLegalEntities.First().AccountLegalEntity, Is.Not.Null);
            Assert.That(resultEntity?.AccountProviderLegalEntities.First().Permissions, Has.Exactly(1).Items);
        });
    }

    [Test]
    public async Task GetAccountLegalEntity_Returns_AccountLegalEntity()
    {
        AccountLegalEntity accountLegalEntity = AccountLegalEntityTestData.Create();

        AccountLegalEntity? result = new();

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(GetAccountLegalEntity_Returns_AccountLegalEntity)}")
        )
        {
            await context.AddAsync(accountLegalEntity);
            await context.SaveChangesAsync(cancellationToken);

            AccountLegalEntityReadRepository sut = new(context);

            result = await sut.GetAccountLegalEntity(1, cancellationToken);
        }

        Assert.That(result, Is.Not.Null, "result should not be null");
    }

    [Test]
    public async Task GetAccountLegalEntity_Returns_Null()
    {
        AccountLegalEntity? result = new();

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(GetAccountLegalEntity_Returns_Null)}")
        )
        {
            AccountLegalEntityReadRepository sut = new(context);

            result = await sut.GetAccountLegalEntity(1, cancellationToken);
        }

        Assert.That(result, Is.Null, "result should be null");
    }

    [Test]
    public async Task AccountLegalEntityExists_Returns_True()
    {
        AccountLegalEntity accountLegalEntity = AccountLegalEntityTestData.Create();

        bool result = false;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(AccountLegalEntityExists_Returns_True)}")
        )
        {
            await context.AddAsync(accountLegalEntity);
            await context.SaveChangesAsync(cancellationToken);

            AccountLegalEntityReadRepository sut = new(context);

            result = await sut.AccountLegalEntityExists(accountLegalEntity.Id, cancellationToken);
        }

        Assert.That(result, Is.True, "result should be true");
    }
}
