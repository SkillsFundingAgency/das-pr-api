using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Application.Requests.Queries.GetRequest;
using SFA.DAS.PR.Data.Repositories;
using SFA.DAS.PR.Data.UnitTests.InMemoryDatabases;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.PR.Domain.Models;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Application.UnitTests.Requests.Queries.GetRequest;

public class GetRequestQueryHandlerTests
{
    [Test]
    [RecursiveMoqAutoData]
    public async Task GetRequestQueryHandler_Handle_ReturnsRequestModel(Request request)
    {
        GetRequestQuery query = new GetRequestQuery(request.Id);

        ValidatedResponse<RequestModel?> requestModel;

        Request persistedRequest;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(GetRequestQueryHandler_Handle_ReturnsRequestModel)}")
        )
        {
            await context.AddAsync(request, CancellationToken.None);
            await context.SaveChangesAsync(CancellationToken.None);

            RequestReadRepository requestReadRepository = new(context);
            GetRequestQueryHandler sut = new(requestReadRepository);
            requestModel = await sut.Handle(query, CancellationToken.None);
            persistedRequest = await context.Requests.FirstAsync(CancellationToken.None);
        }

        requestModel.Result.Should().NotBeNull();

        Assert.Multiple(() =>
        {
            Assert.That(requestModel.Result!.RequestId, Is.EqualTo(persistedRequest.Id));
            Assert.That(requestModel.Result!.RequestType, Is.EqualTo(persistedRequest.RequestType.ToString()));
            Assert.That(requestModel.Result!.Ukprn, Is.EqualTo(persistedRequest.Ukprn));
            Assert.That(requestModel.Result!.ProviderName, Is.EqualTo(persistedRequest.Provider.Name));
            Assert.That(requestModel.Result!.ProviderName, Is.EqualTo(persistedRequest.Provider.Name));
            Assert.That(requestModel.Result!.RequestedBy, Is.EqualTo(persistedRequest.RequestedBy));
            Assert.That(requestModel.Result!.RequestedDate, Is.EqualTo(persistedRequest.RequestedDate));
            Assert.That(requestModel.Result!.AccountLegalEntityId, Is.EqualTo(persistedRequest.AccountLegalEntityId));
            Assert.That(requestModel.Result!.EmployerOrganisationName, Is.EqualTo(persistedRequest.EmployerOrganisationName));
            Assert.That(requestModel.Result!.EmployerContactFirstName, Is.EqualTo(persistedRequest.EmployerContactFirstName));
            Assert.That(requestModel.Result!.EmployerContactLastName, Is.EqualTo(persistedRequest.EmployerContactLastName));
            Assert.That(requestModel.Result!.EmployerContactEmail, Is.EqualTo(persistedRequest.EmployerContactEmail));
            Assert.That(requestModel.Result!.EmployerPAYE, Is.EqualTo(persistedRequest.EmployerPAYE));
            Assert.That(requestModel.Result!.EmployerAORN, Is.EqualTo(persistedRequest.EmployerAORN));
            Assert.That(requestModel.Result!.UpdatedDate, Is.EqualTo(persistedRequest.UpdatedDate));
            Assert.That(requestModel.Result!.Operations, Is.EqualTo(persistedRequest.PermissionRequests.Select(a => (Operation)a.Operation).ToArray()));
            Assert.That(requestModel.Result!.Status, Is.EqualTo(persistedRequest.Status.ToString()));
        });
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task GetRequestQueryHandler_Handle_ReturnsNull(
        [Frozen]Mock<IRequestReadRepository> requestReadRepository,
        GetRequestQueryHandler sut
    )
    {
        requestReadRepository.Setup(a => 
            a.GetRequest(
                It.IsAny<Guid>(), 
                It.IsAny<CancellationToken>()
            )
        )
        .ReturnsAsync((Request?)null);

        GetRequestQuery query = new GetRequestQuery(Guid.NewGuid());

        ValidatedResponse<RequestModel?> requestModel = await sut.Handle(query, CancellationToken.None);

        requestModel.Result.Should().BeNull();
    }
}
