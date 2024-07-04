using AutoFixture;
using NUnit.Framework;
using SFA.DAS.PR.Data.Repositories;
using SFA.DAS.PR.Data.UnitTests.InMemoryDatabases;
using SFA.DAS.PR.Data.UnitTests.Setup;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Data.UnitTests.Repositories;
public class ProvidersReadRepositoryTests
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
    public async Task ProviderExists_Returns_True()
    {
        Provider provider = ProviderTestData.CreateProvider();

        bool result = false;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(ProviderExists_Returns_True)}")
        )
        {
            await context.AddAsync(provider);
            await context.SaveChangesAsync(cancellationToken);

            ProvidersReadRepository sut = new(context);

            result = await sut.ProviderExists(provider.Ukprn, cancellationToken);
        }
        Assert.That(result, Is.True, "result should be true");
    }

    [Test]
    public async Task ProviderDoesNotExist_Returns_False()
    {
        Provider provider = ProviderTestData.CreateProvider();
        var otherUkprn = 12132111;
        bool result = true;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
                   $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(ProviderDoesNotExist_Returns_False)}")
              )
        {
            await context.AddAsync(provider);
            await context.SaveChangesAsync(cancellationToken);

            ProvidersReadRepository sut = new(context);

            result = await sut.ProviderExists(otherUkprn, cancellationToken);
        }
        Assert.That(result, Is.False, "result should be false");
    }
}