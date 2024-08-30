using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Application.Requests.Queries.GetRequest;
using SFA.DAS.PR.Application.Requests.Queries.LookupRequests;
using SFA.DAS.PR.Data.Repositories;
using SFA.DAS.PR.Data.UnitTests.InMemoryDatabases;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.PR.Domain.Models;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Application.UnitTests.Requests.Queries.LookupRequests;

public class LookupRequestsQueryHandlerTests
{
    [Test]
    [RecursiveMoqAutoData]
    public async Task LookupRequestsQueryHandler_Handle_ReturnsRequestModel(Request request)
    {
        ValidatedResponse<RequestModel?> requestModel;

        Request persistedRequest;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(LookupRequestsQueryHandler_Handle_ReturnsRequestModel)}")
        )
        {
            await context.AddAsync(request, CancellationToken.None);
            await context.SaveChangesAsync(CancellationToken.None);

            RequestReadRepository requestReadRepository = new(context);
            LookupRequestsQueryHandler sut = new(requestReadRepository);
            requestModel = await sut.Handle(new(request.Provider.Ukprn, request.EmployerPAYE!), CancellationToken.None);
            persistedRequest = await context.Requests.FirstAsync(CancellationToken.None);
        }

        requestModel.Result.Should().NotBeNull();

        requestModel.Result.Should().BeEquivalentTo(
            persistedRequest,
            options => options
                .Excluding(model => model.Id)
                .Excluding(model => model.RequestType)
                .Excluding(model => model.Provider)
                .Excluding(model => model.PermissionRequests)
                .Excluding(model => model.Status)
                .Excluding(model => model.ActionedBy)
                .Excluding(model => model.AccountLegalEntity)
        );

        Assert.Multiple(() =>
        {
            Assert.That(requestModel.Result!.RequestId, Is.EqualTo(persistedRequest.Id));
            Assert.That(requestModel.Result!.RequestType, Is.EqualTo(persistedRequest.RequestType.ToString()));
            Assert.That(requestModel.Result!.ProviderName, Is.EqualTo(persistedRequest.Provider.Name));
            Assert.That(requestModel.Result!.Operations, Is.EqualTo(persistedRequest.PermissionRequests.Select(a => (Operation)a.Operation).ToArray()));
            Assert.That(requestModel.Result!.Status, Is.EqualTo(persistedRequest.Status.ToString()));
        });
    }

    [Test]
    [RecursiveMoqAutoData]
    public async Task LookupRequestsQueryHandler_Handle_ReturnsNull(
        [Frozen] Mock<IRequestReadRepository> requestReadRepository,
        LookupRequestsQueryHandler sut,
        LookupRequestsQuery query
    )
    {
        requestReadRepository.Setup(a =>
            a.GetRequest(
                It.IsAny<long>(),
                It.IsAny<string>(),
                It.IsAny<RequestStatus[]>(),
                It.IsAny<CancellationToken>()
            )
        )
        .ReturnsAsync((Request?)null);

        ValidatedResponse<RequestModel?> requestModel = await sut.Handle(query, CancellationToken.None);

        requestModel.Result.Should().BeNull();
    }
}
