using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.PR.Api.Common;
using SFA.DAS.PR.Api.Controllers;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Application.Permissions.Queries.GetPermissionsForProviderOnAccount;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Api.UnitTests.Controllers;
public class PermissionsControllerGetTests
{
    [Test, MoqAutoData]
    public async Task HasPermission_InvokesQueryHandler(
      [Frozen] Mock<IMediator> mediatorMock,
      [Greedy] PermissionsController sut,
      GetPermissionsForProviderOnAccountQuery query,
      CancellationToken cancellationToken
   )
    {
        await sut.GetPermissionsForProviderOnAccount(query, cancellationToken);

        mediatorMock.Verify(m =>
            m.Send(It.Is<GetPermissionsForProviderOnAccountQuery>(q =>
                    q.Ukprn == query.Ukprn
                    && q.PublicHashedId == query.PublicHashedId),
            cancellationToken)
        );
    }

    [Test, MoqAutoData]
    public async Task HasPermission_HandlerReturnsData_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] PermissionsController sut,
        GetPermissionsForProviderOnAccountQuery query,
        GetPermissionsForProviderOnAccountQueryResult getPermissionsResult,
        CancellationToken cancellationToken
    )
    {
        var response = new ValidatedResponse<GetPermissionsForProviderOnAccountQueryResult>(getPermissionsResult);

        mediatorMock.Setup(m =>
                m.Send(It.Is<GetPermissionsForProviderOnAccountQuery>(q =>
                        q.Ukprn == query.Ukprn
                        && q.PublicHashedId == query.PublicHashedId),
                    cancellationToken)
        ).ReturnsAsync(response);

        var result = await sut.GetPermissionsForProviderOnAccount(query, cancellationToken);
        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().Be(getPermissionsResult);
    }

    [Test, MoqAutoData]
    public async Task HasPermission_InvalidRequest_ReturnsBadRequestResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] PermissionsController sut,
        GetPermissionsForProviderOnAccountQuery query,
        List<ValidationFailure> errors,
        CancellationToken cancellationToken
    )
    {
        var errorResponse = new ValidatedResponse<GetPermissionsForProviderOnAccountQueryResult>(errors);

        mediatorMock.Setup(m =>
            m.Send(It.IsAny<GetPermissionsForProviderOnAccountQuery>(),
            cancellationToken)
        ).ReturnsAsync(errorResponse);

        var result = await sut.GetPermissionsForProviderOnAccount(query, cancellationToken);
        result.As<BadRequestObjectResult>().Should().NotBeNull();
        result.As<BadRequestObjectResult>().Value.As<List<ValidationError>>().Count.Should().Be(errors.Count);
    }
}