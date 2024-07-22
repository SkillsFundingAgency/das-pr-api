using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.PR.Api.Common;
using SFA.DAS.PR.Api.Controllers;
using SFA.DAS.PR.Api.Models;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Application.ProviderRelationships.Queries.GetProviderRelationships;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Api.UnitTests.Controllers;

public class ProviderRelationshipsControllerTests
{
    [Test, MoqAutoData]
    public async Task GetProviderRelationships_InvokesQueryHandler(
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ProviderRelationshipsController sut,
        ProviderRelationshipsRequestModel requestModel,
        long ukprn,
        CancellationToken cancellationToken)
    {
        await sut.GetProviderRelationships(ukprn, requestModel, cancellationToken);

        mockMediator.Verify(m => m.Send(
            It.Is<GetProviderRelationshipsQuery>(q =>
                q.Ukprn == ukprn
                && q.EmployerName == requestModel.EmployerName
                && q.HasRecruitPermission == requestModel.HasRecruitmentPermission
                && q.HasRecruitWithReviewPermission == requestModel.HasRecruitmentWithReviewPermission
                && q.HasCreateCohortPermission == requestModel.HasCreateCohortPermission
                && q.HasPendingRequest == requestModel.HasPendingRequest
                && q.PageNumber == requestModel.PageNumber
                && q.PageSize == requestModel.PageSize),
            cancellationToken));
    }

    [Test, MoqAutoData]
    public async Task GetProviderRelationships_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ProviderRelationshipsController sut,
        ProviderRelationshipsRequestModel requestModel,
        long ukprn,
        GetProviderRelationshipsQueryResult expectedResult,
        CancellationToken cancellationToken)
    {
        mockMediator.Setup(m => m.Send(It.IsAny<GetProviderRelationshipsQuery>(), cancellationToken)).ReturnsAsync(new ValidatedResponse<GetProviderRelationshipsQueryResult>(expectedResult));

        var result = await sut.GetProviderRelationships(ukprn, requestModel, cancellationToken);

        result.As<OkObjectResult>().Value.Should().Be(expectedResult);
    }

    [Test, MoqAutoData]
    public async Task GetProviderRelationships_ReturnsDabRequestResponse(
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ProviderRelationshipsController sut,
        ProviderRelationshipsRequestModel requestModel,
        List<ValidationFailure> errors,
        long ukprn,
        CancellationToken cancellationToken)
    {
        mockMediator.Setup(m => m.Send(It.IsAny<GetProviderRelationshipsQuery>(), cancellationToken)).ReturnsAsync(new ValidatedResponse<GetProviderRelationshipsQueryResult>(errors));

        var result = await sut.GetProviderRelationships(ukprn, requestModel, cancellationToken);

        result.As<BadRequestObjectResult>().Value.As<List<ValidationError>>().Count.Should().Be(errors.Count);
    }
}
