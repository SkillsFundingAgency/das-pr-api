using NUnit.Framework;
using SFA.DAS.PR.Data.Repositories;
using SFA.DAS.PR.Data.UnitTests.InMemoryDatabases;
using SFA.DAS.PR.Data.UnitTests.Setup;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Data.UnitTests.Repositories;

public class EmployerRelationshipsReadRepositoryTests
{
    private CancellationToken cancellationToken = CancellationToken.None;

    [Test]
    public async Task GetRelationships_Returns_Permissions()
    {
        Account account = AccountTestData.CreateAccount(1004);

        List<AccountProviderLegalEntity> accountProviderLegalEntitiesToAdd =
            AccountProviderLegalEntityTestData.CreateAccountProviderLegalEntitiesWithPermissions(account);

        Account? result = new();

        await using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(GetRelationships_Returns_Permissions)}")
        )
        {
            await context.AddAsync(account);
            await context.AddRangeAsync(accountProviderLegalEntitiesToAdd);
            await context.SaveChangesAsync(cancellationToken);

            EmployerRelationshipsReadRepository sut = new(context);

            result = await sut.GetRelationships(account.Id, cancellationToken);
        }

        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null, "result should not be null");
            Assert.That(result?.AccountLegalEntities, Has.Count.EqualTo(1), "AccountLegalEntities count should be 1");
            Assert.That(result?.AccountProviders, Has.Count.EqualTo(1), "AccountProviders count should be 1");
            Assert.That(result?.AccountLegalEntities
                           .SelectMany(a => a.AccountProviderLegalEntities)
                           .SelectMany(a => a.Permissions)
                           .ToList(),
                        Has.Count.EqualTo(1), "Permissions count should be 1");
            Assert.That(result?.AccountProviders
                           .Select(a => a.Provider)
                           .Select(a => a.Requests)
                           .ToList(),
                        Has.Count.EqualTo(1), "Requests count should be 1");
        });
    }
}
