using AutoFixture;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.PR.Data.Repositories;
using SFA.DAS.PR.Data.UnitTests.InMemoryDatabases;
using SFA.DAS.PR.Data.UnitTests.Setup;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Data.UnitTests.Repositories;

public class AccountWriteRepositoryTests
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
    public async Task AccountWriteRepository_CreateAccount_Valid()
    {
        Account account = AccountTestData.CreateAccount(1);

        Account? result = null;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(AccountWriteRepository_CreateAccount_Valid)}")
        )
        {
            AccountWriteRepository sut = new(context);

            await sut.CreateAccount(account, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            result = await context.Accounts.FirstOrDefaultAsync(a => a.Id == account.Id, cancellationToken);
        }

        Assert.That(result, Is.Not.Null);
    }
}
