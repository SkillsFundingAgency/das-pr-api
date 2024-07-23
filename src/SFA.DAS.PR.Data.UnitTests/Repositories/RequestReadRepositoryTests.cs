using NUnit.Framework;
using SFA.DAS.PR.Data.Repositories;
using SFA.DAS.PR.Data.UnitTests.InMemoryDatabases;
using SFA.DAS.PR.Data.UnitTests.Setup;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Extensions;

namespace SFA.DAS.PR.Data.UnitTests.Repositories;

public class RequestReadRepositoryTests
{
    private readonly CancellationToken cancellationToken = CancellationToken.None;

    [Test]
    public async Task GetRequest_Returns_Success()
    {
        Request request = RequestTestData.Create(Guid.NewGuid());

        Request? result = null;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(GetRequest_Returns_Success)}")
        )
        {
            await context.AddAsync(request);
            await context.SaveChangesAsync(cancellationToken);

            RequestReadRepository sut = new(context);

            result = await sut.GetRequest(request.Ukprn, request.AccountLegalEntityId!.Value, cancellationToken);
        }

        Assert.That(result, Is.Not.Null, $"result should not be null");
    }

    [Test]
    public async Task GetRequest_Returns_Null()
    {
        Request? result = null;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(GetRequest_Returns_Null)}")
        )
        {
            RequestReadRepository sut = new(context);

            result = await sut.GetRequest(10000001, 1, cancellationToken);
        }

        Assert.That(result, Is.Null, $"result should be null");
    }

    [Test]
    public async Task RequestExists_RequestStatusNew_Returns_True()
    {
        Request request = RequestTestData.Create(Guid.NewGuid(), RequestStatus.New.ToLowerString());

        bool? result = null;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(RequestExists_RequestStatusNew_Returns_True)}")
        )
        {
            await context.AddAsync(request);
            await context.SaveChangesAsync(cancellationToken);

            RequestReadRepository sut = new(context);

            result = await sut.RequestExists(request.Ukprn, request.AccountLegalEntityId!.Value, cancellationToken);
        }

        Assert.That(result, Is.True, $"result should be True");
    }

    [Test]
    public async Task RequestExists_RequestStatusSent_Returns_True()
    {
        Request request = RequestTestData.Create(Guid.NewGuid(), RequestStatus.Sent.ToLowerString());

        bool? result = null;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(RequestExists_RequestStatusSent_Returns_True)}")
        )
        {
            await context.AddAsync(request);
            await context.SaveChangesAsync(cancellationToken);

            RequestReadRepository sut = new(context);

            result = await sut.RequestExists(request.Ukprn, request.AccountLegalEntityId!.Value, cancellationToken);
        }

        Assert.That(result, Is.True, $"result should be True");
    }

    [Test]
    public async Task RequestExists_RequestStatusSent_Returns_False()
    {
        bool? result = null;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(RequestExists_RequestStatusSent_Returns_True)}")
        )
        {
            RequestReadRepository sut = new(context);

            result = await sut.RequestExists(10000001, 1, cancellationToken);
        }

        Assert.That(result, Is.False, $"result should be False");
    }
}