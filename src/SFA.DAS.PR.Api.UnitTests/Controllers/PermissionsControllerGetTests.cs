using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.PR.Api.Common;
using SFA.DAS.PR.Api.Controllers;
using SFA.DAS.PR.Api.RouteValues.Permissions;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Application.Permissions.Queries.GetPermissionsHas;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Api.UnitTests.Controllers;
public class PermissionsControllerGetTests
{
    [Test, MoqAutoData]
    public async Task HasPermission_InvokesQueryHandler(
      [Frozen] Mock<IMediator> mediatorMock,
      [Greedy] PermissionsController sut,
      HasPermissionRouteValues routeValues,
      CancellationToken cancellationToken
   )
    {
        await sut.HasPermission(routeValues, cancellationToken);

        mediatorMock.Verify(m =>
            m.Send(It.Is<GetPermissionsHasQuery>(q =>
                    q.Ukprn == routeValues.Ukprn
                    && q.PublicHashedId == routeValues.PublicHashedId
                    && q.Operations == routeValues.Operations),
            cancellationToken)
        );
    }

    [Test, MoqAutoData]
    public async Task HasPermission_HandlerReturnsNullResult_ReturnsNotFoundResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] PermissionsController sut,
        HasPermissionRouteValues routeValues,
        CancellationToken cancellationToken
    )
    {
        var notFoundResponse = ValidatedResponse<GetPermissionsHasResult>.EmptySuccessResponse();

        mediatorMock.Setup(m =>
            m.Send(It.Is<GetPermissionsHasQuery>(q =>
                    q.Ukprn == routeValues.Ukprn
                    && q.PublicHashedId == routeValues.PublicHashedId
                    && q.Operations == routeValues.Operations),
            cancellationToken)
        ).ReturnsAsync(notFoundResponse);

        var result = await sut.HasPermission(routeValues, cancellationToken);
        result.As<NotFoundResult>().Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task HasPermission_HandlerReturnsData_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] PermissionsController sut,
        HasPermissionRouteValues routeValues,
        GetPermissionsHasResult getPermissionsHasResult,
        CancellationToken cancellationToken
    )
    {
        var response = new ValidatedResponse<GetPermissionsHasResult>(getPermissionsHasResult);

        mediatorMock.Setup(m =>
                m.Send(It.Is<GetPermissionsHasQuery>(q =>
                        q.Ukprn == routeValues.Ukprn
                        && q.PublicHashedId == routeValues.PublicHashedId
                        && q.Operations == routeValues.Operations),
                    cancellationToken)
        ).ReturnsAsync(response);

        var result = await sut.HasPermission(routeValues, cancellationToken);
        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().Be(getPermissionsHasResult);
    }

    [Test, MoqAutoData]
    public async Task HasPermission_InvalidRequest_ReturnsBadRequestResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] PermissionsController sut,
        HasPermissionRouteValues routeValues,
        List<ValidationFailure> errors,
        CancellationToken cancellationToken
    )
    {
        var errorResponse = new ValidatedResponse<GetPermissionsHasResult>(errors);

        mediatorMock.Setup(m =>
            m.Send(It.IsAny<GetPermissionsHasQuery>(),
            cancellationToken)
        ).ReturnsAsync(errorResponse);

        var result = await sut.HasPermission(routeValues, cancellationToken);
        result.As<BadRequestObjectResult>().Should().NotBeNull();
        result.As<BadRequestObjectResult>().Value.As<List<ValidationError>>().Count.Should().Be(errors.Count);
    }
}