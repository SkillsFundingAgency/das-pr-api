using AutoFixture;
using AutoFixture.NUnit3;
using NUnit.Framework;
using SFA.DAS.PR.Data.Repositories;
using SFA.DAS.PR.Data.UnitTests.InMemoryDatabases;
using SFA.DAS.PR.Data.UnitTests.Setup;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Data.UnitTests.Repositories;

public sealed class AccountReadRepositoryTests
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
    public async Task AccountReadRepositoryTests_GetAccount_Valid()
    {
        Account account = AccountTestData.CreateAccount(1);

        Account? result = null;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(AccountReadRepositoryTests_GetAccount_Valid)}")
        )
        {
            await context.Accounts.AddAsync(account, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            AccountReadRepository sut = new(context);

            result = await sut.GetAccount(account.Id, cancellationToken);
        }

        Assert.That(result, Is.Not.Null);
    }

    [Test]
    [InlineAutoData(123456, 123456, true)]
    [InlineAutoData(654321, 1234567, false)]
    public async Task AccountIdExists_Returns_Expected(long accountIdToCheck, long accountIdExisting, bool expected)
    {
        Account account = AccountTestData.CreateAccount(accountIdExisting);

        bool result;

        await using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
                         $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(AccountIdExists_Returns_Expected)}")
                    )
        {
            await context.AddAsync(account);
            await context.SaveChangesAsync(cancellationToken);

            AccountReadRepository sut = new(context);

            result = await sut.AccountIdExists(accountIdToCheck, cancellationToken);
        }

        Assert.That(result, Is.EqualTo(expected));
    }
}
