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

            result = await sut.GetAccountLegalEntiies(accountId, cancellationToken);
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
}
