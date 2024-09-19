using NUnit.Framework;
using SFA.DAS.PR.Data.Repositories;
using SFA.DAS.PR.Data.UnitTests.InMemoryDatabases;
using SFA.DAS.PR.Data.UnitTests.Setup;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Data.UnitTests.Repositories;

public class AccountProviderLegalEntitiesWriteRepositoryTests
{
    private readonly CancellationToken cancellationToken = CancellationToken.None;

    [Test]
    public async Task CreateAccountProviderLegalEntity_Returns_Success()
    {
        AccountProvider accountProvider = AccountProviderTestData.CreateAccountProvider(1, 1, 1);

        AccountProviderLegalEntity? result = new();

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(CreateAccountProviderLegalEntity_Returns_Success)}")
        )
        {
            AccountProviderLegalEntitiesWriteRepository sut = new(context);

            await sut.CreateAccountProviderLegalEntity(new AccountProviderLegalEntity() { Id = 1, AccountProvider = accountProvider }, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            result = context.AccountProviderLegalEntities.FirstOrDefault(a => a.Id == 1);
        }

        Assert.That(result, Is.Not.Null, "result should not be null");
    }
}
