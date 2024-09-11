using NUnit.Framework;
using SFA.DAS.PR.Data.Repositories;
using SFA.DAS.PR.Data.UnitTests.InMemoryDatabases;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Data.UnitTests.Repositories;

public class RequestWriteRepositoryTests
{
    private readonly CancellationToken cancellationToken = CancellationToken.None;

    [Test]
    [RecursiveMoqAutoData]
    public async Task CreateRequest_Returns_Success(Request request)
    {
        Request? persistedRequest = null;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(CreateRequest_Returns_Success)}")
        )
        {
            RequestWriteRepository sut = new(context);

            await sut.CreateRequest(request, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            persistedRequest = context.Requests.First();
        }

        Assert.That(persistedRequest, Is.Not.Null, $"request should not be null");
        Assert.That(persistedRequest.PermissionRequests, Has.Count.EqualTo(request.PermissionRequests.Count), $"permission requests count should not be {request.PermissionRequests.Count}");
    }
}
