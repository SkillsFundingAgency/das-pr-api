using AutoFixture;
using NUnit.Framework;
using SFA.DAS.PR.Data.Repositories;
using SFA.DAS.PR.Data.UnitTests.InMemoryDatabases;
using SFA.DAS.PR.Data.UnitTests.Setup;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Data.UnitTests.AccountWithPermissions;

public class AccountWithMultiplePermission
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
    public async Task Account_With_Permissions_Is_Returned()
    {
        Account account = AccountTestData.CreateAccount(1004);

        List<AccountProviderLegalEntity> accountProviderLegalEntitiesToAdd =
            AccountProviderLegalEntityTestData.CreateAccountProviderLegalEntitiesWithPermissions(account);

        Account? result = new();

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(Account_With_Permissions_Is_Returned)}")
        )
        {
            await context.AddAsync(account);
            await context.AddRangeAsync(accountProviderLegalEntitiesToAdd);
            await context.SaveChangesAsync(cancellationToken);

            EmployerRelationshipsReadRepository sut = new(context);

            result = await sut.GetRelationships(account.HashedId, cancellationToken);
        }

        var accountProvidersCount = accountProviderLegalEntitiesToAdd.Count();

        Assert.That(result, Is.Not.Null);
        Assert.That(result.AccountLegalEntities.Count, Is.EqualTo(1));
        Assert.That(result.AccountProviders.Count, Is.EqualTo(1));
        Assert.That(result.AccountLegalEntities.SelectMany(a => a.AccountProviderLegalEntities).SelectMany(a => a.Permissions).Count, Is.EqualTo(1));
    }
}
