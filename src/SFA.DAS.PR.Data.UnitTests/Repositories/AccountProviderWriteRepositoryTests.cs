using NUnit.Framework;
using SFA.DAS.PR.Data.Repositories;
using SFA.DAS.PR.Data.UnitTests.InMemoryDatabases;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Data.UnitTests.Repositories;

public class AccountProviderWriteRepositoryTests
{
    private readonly CancellationToken cancellationToken = CancellationToken.None;

    [Test]
    public async Task CreateAccountProvider_Returns_Success()
    {
        AccountProvider? result = new();

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(CreateAccountProvider_Returns_Success)}")
        )
        {
            AccountProviderWriteRepository sut = new(context);

            await sut.CreateAccountProvider(1, 1, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            result = context.AccountProviders.FirstOrDefault(a => a.Id == 1);
        }

        Assert.That(result, Is.Not.Null, "result should not be null");
    }
}