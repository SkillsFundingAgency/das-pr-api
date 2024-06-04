using NUnit.Framework;
using SFA.DAS.PR.Data.Repositories;
using SFA.DAS.PR.Data.UnitTests.InMemoryDatabases;
using SFA.DAS.PR.Data.UnitTests.Setup;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Data.UnitTests.Repositories;

public class AccountLegalEntityReadRepositoryTests
{
    private readonly CancellationToken cancellationToken = CancellationToken.None;

    [Test]
    public async Task GetAccountLegalEntity_Returns_AccountLegalEntity()
    {
        AccountLegalEntity accountLegalEntity = AccountLegalEntityTestData.CreateAccountLegalEntity();

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
}
