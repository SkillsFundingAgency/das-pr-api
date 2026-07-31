using NUnit.Framework;
using SFA.DAS.PR.Data.Repositories;
using SFA.DAS.PR.Data.UnitTests.InMemoryDatabases;
using SFA.DAS.PR.Data.UnitTests.Setup;
using SFA.DAS.PR.Domain.Common;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Data.UnitTests.Repositories;

public class ProviderReadRepositoryTests
{
    private readonly CancellationToken cancellationToken = CancellationToken.None;

    [Test]
    public async Task ProviderExists_Returns_True()
    {
        bool result = false;

        Provider provider = ProviderTestData.Create(10000001);

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(ProviderExists_Returns_True)}")
        )
        {
            await context.AddAsync(provider);
            await context.SaveChangesAsync(cancellationToken);

            ProviderReadRepository sut = new(context);

            result = await sut.ProviderExists(10000001, cancellationToken);
        }

        Assert.That(result, Is.True, $"result should be true");
    }

    [Test]
    public async Task ProviderExists_Returns_False()
    {
        bool result = true;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(ProviderExists_Returns_False)}")
        )
        {
            ProviderReadRepository sut = new(context);

            result = await sut.ProviderExists(10000001, cancellationToken);
        }

        Assert.That(result, Is.False, $"result should be false");
    }

    [Test]
    public async Task ProviderRemoved_Returns_True_When_Provider_Is_Removed()
    {
        bool result = false;

        Provider provider = ProviderTestData.Create(10000001);
        provider.Status = ProviderStatus.Removed;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(ProviderRemoved_Returns_True_When_Provider_Is_Removed)}"))
        {
            await context.AddAsync(provider);
            await context.SaveChangesAsync(cancellationToken);

            ProviderReadRepository sut = new(context);

            result = await sut.ProviderRemoved(10000001, cancellationToken);
        }

        Assert.That(result, Is.True, "result should be true");
    }

    [Test]
    public async Task ProviderRemoved_Returns_False_When_Provider_Is_Not_Removed()
    {
        bool result = true;

        Provider provider = ProviderTestData.Create(10000001);
        provider.Status = null;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(ProviderRemoved_Returns_False_When_Provider_Is_Not_Removed)}"))
        {
            await context.AddAsync(provider);
            await context.SaveChangesAsync(cancellationToken);

            ProviderReadRepository sut = new(context);

            result = await sut.ProviderRemoved(10000001, cancellationToken);
        }

        Assert.That(result, Is.False, "result should be false");
    }
}
