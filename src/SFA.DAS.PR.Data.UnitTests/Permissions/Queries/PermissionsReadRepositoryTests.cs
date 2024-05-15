using NUnit.Framework;
using SFA.DAS.PR.Data.Repositories;
using SFA.DAS.PR.Data.UnitTests.InMemoryDatabases;
using SFA.DAS.PR.Data.UnitTests.Setup;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Data.UnitTests.Permissions.Queries;
public class PermissionsReadRepositoryTests
{
    private readonly CancellationToken _cancellationToken = CancellationToken.None;

    [Test]
    public async Task AccountProviderLegalEntities_Has_Operations()
    {
        Account account = AccountTestData.CreateAccount(1004);

        List<AccountProviderLegalEntity> accountProviderLegalEntitiesToAdd =
            AccountProviderLegalEntityTestData.CreateAccountProviderLegalEntitiesWithPermissions(account);

        long ukprn = accountProviderLegalEntitiesToAdd.First().AccountProvider.ProviderUkprn;
        string publicHashedId = accountProviderLegalEntitiesToAdd.First().AccountLegalEntity.PublicHashedId;

        List<Operation> expectedOperations;
        var actualOperations = accountProviderLegalEntitiesToAdd.First().Permissions.Select(x => x.Operation).ToList();

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
                   contextName: $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(AccountProviderLegalEntities_Has_Operations)}")
              )
        {
            await context.AddAsync(account);
            await context.AddRangeAsync(accountProviderLegalEntitiesToAdd);
            await context.SaveChangesAsync(_cancellationToken);

            PermissionsReadRepository sut = new(context);

            expectedOperations = await sut.GetOperations(ukprn, publicHashedId, _cancellationToken);
        }

        Assert.AreEqual(expectedOperations, actualOperations);
    }
}
