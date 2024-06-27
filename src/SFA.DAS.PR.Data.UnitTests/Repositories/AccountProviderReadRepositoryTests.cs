using NUnit.Framework;
using SFA.DAS.PR.Data.Repositories;
using SFA.DAS.PR.Data.UnitTests.InMemoryDatabases;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Data.UnitTests.Repositories;

public class AccountProviderReadRepositoryTests
{
    private readonly IEnumerable<AccountProvider> _accountProviders = Enumerable.Range(1, 3).Select(i => new AccountProvider { AccountId = i * 10, Id = i, ProviderUkprn = 10000000 + i });

    [TestCase(10000001, 10, true)]
    [TestCase(10000002, 20, true)]
    [TestCase(10000003, 30, true)]
    [TestCase(10000001, 30, false)]
    public async Task GetAccountProvider_RelationshipExist_ReturnsEntity(long ukprn, long accountId, bool hasMatch)
    {
        using var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext();

        context.AccountProviders.AddRange(_accountProviders);
        await context.SaveChangesAsync();

        AccountProviderReadRepository sut = new(context);

        var result = await sut.GetAccountProvider(ukprn, accountId, CancellationToken.None);

        if (hasMatch)
        {
            Assert.That(result, Is.Not.Null);
        }
        else
        {
            Assert.That(result, Is.Null);
        }
    }
}
