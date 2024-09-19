using NUnit.Framework;
using SFA.DAS.PR.Data.Repositories;
using SFA.DAS.PR.Data.UnitTests.InMemoryDatabases;
using SFA.DAS.PR.Data.UnitTests.Setup;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Data.UnitTests.Repositories;

public class RequestReadRepositoryTests
{
    private readonly CancellationToken cancellationToken = CancellationToken.None;

    [Test]
    public async Task GetRequestById_Returns_Success()
    {
        Request request = RequestTestData.Create(Guid.NewGuid());

        Request? result = null;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(GetRequestById_Returns_Success)}")
        )
        {
            await context.AddAsync(request);
            await context.SaveChangesAsync(cancellationToken);

            RequestReadRepository sut = new(context);

            result = await sut.GetRequest(request.Id, cancellationToken);
        }

        Assert.That(result, Is.Not.Null, "result should not be null");
    }

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

        Assert.That(result, Is.Not.Null, "result should not be null");
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

        Assert.That(result, Is.Null, "result should be null");
    }

    [Test]
    public async Task GetRequestByUkprnPaye_Returns_Success()
    {
        Request request = RequestTestData.Create(Guid.NewGuid());

        Request? result = null;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(GetRequestByUkprnPaye_Returns_Success)}")
        )
        {
            await context.AddAsync(request);
            await context.SaveChangesAsync(cancellationToken);

            RequestReadRepository sut = new(context);

            result = await sut.GetRequest(request.Provider.Ukprn, request.EmployerPAYE!, [RequestStatus.New], cancellationToken);
        }

        Assert.That(result, Is.Not.Null, "result should not be null");
    }

    [Test]
    public async Task GetRequestByUkprnPaye_EmptyStatuses_Returns_Success()
    {
        Request request = RequestTestData.Create(Guid.NewGuid());

        Request? result = null;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(GetRequestByUkprnPaye_EmptyStatuses_Returns_Success)}")
        )
        {
            await context.AddAsync(request);
            await context.SaveChangesAsync(cancellationToken);

            RequestReadRepository sut = new(context);

            result = await sut.GetRequest(request.Provider.Ukprn, request.EmployerPAYE!, [], cancellationToken);
        }

        Assert.That(result, Is.Not.Null, "result should not be null");
    }

    [Test]
    public async Task GetRequestByUkprnPaye_EmptyStatuses_Returns_Null()
    {
        Request? result = null;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(GetRequestByUkprnPaye_Returns_Success)}")
        )
        {
            RequestReadRepository sut = new(context);

            result = await sut.GetRequest(10000001, "Paye", [], cancellationToken);
        }

        Assert.That(result, Is.Null, "result should be null");
    }

    [Test]
    public async Task RequestExists_RequestStatusNew_Returns_True()
    {
        Request request = RequestTestData.Create(Guid.NewGuid());

        bool? result = null;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(RequestExists_RequestStatusNew_Returns_True)}")
        )
        {
            await context.AddAsync(request);
            await context.SaveChangesAsync(cancellationToken);

            RequestReadRepository sut = new(context);

            result = await sut.RequestExists(request.Ukprn, request.AccountLegalEntityId!.Value, [RequestStatus.New], cancellationToken);
        }

        Assert.That(result, Is.True, "result should be True");
    }

    [Test]
    public async Task RequestExists_EmptyRequestStatus_Returns_True()
    {
        Request request = RequestTestData.Create(Guid.NewGuid());

        bool? result = null;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(RequestExists_EmptyRequestStatus_Returns_True)}")
        )
        {
            await context.AddAsync(request);
            await context.SaveChangesAsync(cancellationToken);

            RequestReadRepository sut = new(context);

            result = await sut.RequestExists(request.Ukprn, request.AccountLegalEntityId!.Value, [], cancellationToken);
        }

        Assert.That(result, Is.True, "result should be True");
    }

    [Test]
    public async Task RequestExists_RequestStatusSent_Returns_True()
    {
        Request request = RequestTestData.Create(Guid.NewGuid(), RequestStatus.Sent);

        bool? result = null;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(RequestExists_RequestStatusSent_Returns_True)}")
        )
        {
            await context.AddAsync(request);
            await context.SaveChangesAsync(cancellationToken);

            RequestReadRepository sut = new(context);

            result = await sut.RequestExists(request.Ukprn, request.AccountLegalEntityId!.Value, [RequestStatus.Sent], cancellationToken);
        }

        Assert.That(result, Is.True, "result should be True");
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

            result = await sut.RequestExists(10000001, 1, [RequestStatus.New], cancellationToken);
        }

        Assert.That(result, Is.False, "result should be False");
    }

    [Test]
    public async Task RequestExists_SearchByUkrpnPaye_Returns_True()
    {
        Request request = RequestTestData.Create(Guid.NewGuid(), RequestStatus.Sent);

        bool? result = null;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(RequestExists_SearchByUkrpnPaye_Returns_True)}")
        )
        {
            await context.AddAsync(request);
            await context.SaveChangesAsync(cancellationToken);

            RequestReadRepository sut = new(context);

            result = await sut.RequestExists(request.Ukprn, request.EmployerPAYE!, [RequestStatus.Sent], cancellationToken);
        }

        Assert.That(result, Is.True, "result should be True");
    }

    [Test]
    public async Task RequestExists_SearchByUkrpnPaye_Returns_False()
    {
        bool? result = null;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(RequestExists_SearchByUkrpnPaye_Returns_False)}")
        )
        {
            RequestReadRepository sut = new(context);

            result = await sut.RequestExists(10000001, "EmployerPaye", [RequestStatus.New], cancellationToken);
        }

        Assert.That(result, Is.False, "result should be False");
    }

    [Test]
    public async Task RequestExists_SearchByRequestId_Returns_False()
    {
        bool? result = null;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(RequestExists_SearchByRequestId_Returns_False)}")
        )
        {
            RequestReadRepository sut = new(context);

            result = await sut.RequestExists(Guid.NewGuid(), [RequestStatus.New], RequestType.CreateAccount, cancellationToken);
        }

        Assert.That(result, Is.False, "result should be False");
    }

    [Test]
    public async Task RequestExists_SearchByRequestId_Returns_True()
    {
        Guid requestId = Guid.NewGuid();

        Request request = RequestTestData.Create(requestId, RequestStatus.Sent);

        bool? result = null;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(RequestExists_SearchByRequestId_Returns_True)}")
        )
        {
            await context.AddAsync(request);
            await context.SaveChangesAsync(cancellationToken);

            RequestReadRepository sut = new(context);

            result = await sut.RequestExists(requestId, [RequestStatus.Sent], RequestType.CreateAccount, cancellationToken);
        }

        Assert.That(result, Is.True, "result should be True");
    }
}