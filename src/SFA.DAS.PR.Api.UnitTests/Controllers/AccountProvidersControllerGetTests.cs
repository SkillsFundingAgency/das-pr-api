using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.PR.Api.Common;
using SFA.DAS.PR.Api.Controllers;
using SFA.DAS.PR.Application.AccountProviders.Queries.GetAccountProviders;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Api.UnitTests.Controllers;

public class AccountProvidersControllerGetTests
{
    [Test]
    [MoqAutoData]
    public async Task GetProviders_InvokesQueryHandler(
       [Frozen] Mock<IMediator> mediatorMock,
       [Greedy] AccountProvidersController sut,
       long accountId, 
       CancellationToken cancellationToken
    )
    {
        await sut.Get(accountId, cancellationToken);

        mediatorMock.Verify(m => 
            m.Send(It.Is<GetAccountProvidersQuery>(q => q.AccountId == accountId),
            cancellationToken)
        );
    }

    [Test]
    [MoqAutoData]
    public async Task GetProviders_HandlerReturnsNullResult_ReturnsNotFoundResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] AccountProvidersController sut,
        long accountId,
        CancellationToken cancellationToken
    )
    {
        var notFoundResponse = ValidatedResponse<GetAccountProvidersQueryResult>.EmptySuccessResponse();

        mediatorMock.Setup(m => 
            m.Send(It.Is<GetAccountProvidersQuery>(q => q.AccountId == accountId),
            cancellationToken)
        ).ReturnsAsync(notFoundResponse);

        var result = await sut.Get(accountId, cancellationToken);
        result.As<NotFoundResult>().Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task GetAccountProviders_HandlerReturnsData_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] AccountProvidersController sut,
        long accountId,
        GetAccountProvidersQueryResult getAccountProviderResult,
        CancellationToken cancellationToken
    )
    {
        var response = new ValidatedResponse<GetAccountProvidersQueryResult>(getAccountProviderResult);

        mediatorMock.Setup(m => 
            m.Send(It.Is<GetAccountProvidersQuery>(q => q.AccountId == accountId),
            cancellationToken)
        ).ReturnsAsync(response);

        var result = await sut.Get(accountId, cancellationToken);

        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().Be(getAccountProviderResult);
    }

    [Test, MoqAutoData]
    public async Task GetAccountProviders_InvalidRequest_ReturnsBadRequestResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] AccountProvidersController sut,
        List<ValidationFailure> errors,
        long accountId,
        CancellationToken cancellationToken
    )
    {
        var errorResponse = new ValidatedResponse<GetAccountProvidersQueryResult>(errors);

        mediatorMock.Setup(m => 
            m.Send(It.Is<GetAccountProvidersQuery>(q => q.AccountId == 0),
            cancellationToken)
        ).ReturnsAsync(errorResponse);

        var result = await sut.Get(0, cancellationToken);
        result.As<BadRequestObjectResult>().Should().NotBeNull();
        result.As<BadRequestObjectResult>().Value.As<List<ValidationError>>().Count.Should().Be(errors.Count);
    }
}
