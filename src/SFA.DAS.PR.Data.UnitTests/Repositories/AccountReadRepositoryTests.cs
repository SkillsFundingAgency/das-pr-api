using NUnit.Framework;
using SFA.DAS.PR.Data.Repositories;
using SFA.DAS.PR.Data.UnitTests.InMemoryDatabases;
using SFA.DAS.PR.Data.UnitTests.Setup;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Data.UnitTests.Repositories;

public class AccountReadRepositoryTests
{
    private CancellationToken cancellationToken = CancellationToken.None;

    [Test]
    public async Task AccountExists_Returns_True()
    {
        Account account = AccountTestData.CreateAccount(1004);

        bool result = false;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(AccountExists_Returns_True)}")
        )
        {
            await context.AddAsync(account);
            await context.SaveChangesAsync(cancellationToken);

            AccountReadRepository sut = new(context);

            result = await sut.AccountExists(account.HashedId, cancellationToken);
        }

        Assert.That(result, Is.True, "result should not be true");
    }

    [Test]
    public async Task AccountExists_Returns_False()
    {
        Account account = AccountTestData.CreateAccount(1004);

        bool result = false;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(AccountExists_Returns_False)}")
        )
        {
            await context.AddAsync(account);
            await context.SaveChangesAsync(cancellationToken);

            AccountReadRepository sut = new(context);

            result = await sut.AccountExists("no hash", cancellationToken);
        }

        Assert.That(result, Is.False, "result should not be true");
    }
}
