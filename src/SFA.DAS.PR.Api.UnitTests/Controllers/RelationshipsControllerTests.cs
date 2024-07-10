using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.PR.Api.Controllers;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Application.Relationships.Queries.GetRelationships;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Api.UnitTests.Controllers;

public class RelationshipsControllerTests
{
    private Fixture _fixture = null!;

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Test]
    [MoqAutoData]
    public async Task GetRelationships_InvokesQueryHandler(
       [Frozen] Mock<IMediator> mediatorMock,
       [Greedy] RelationshipsController sut,
       GetRelationshipsQuery query,
       CancellationToken cancellationToken
    )
    {
        await sut.GetRelationships(query.Ukprn, query.AccountLegalEntityId, cancellationToken);

        mediatorMock.Verify(m =>
            m.Send(It.IsAny<GetRelationshipsQuery>(), cancellationToken)
        );
    }

    [Test]
    [MoqAutoData]
    public async Task GetRelationships_HandlerReturnsData_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] RelationshipsController sut,
        GetRelationshipsQuery query,
        GetRelationshipsQueryResult queryResult,
        CancellationToken cancellationToken
    )
    {
        var response = new ValidatedResponse<GetRelationshipsQueryResult?>(queryResult);

        mediatorMock.Setup(m =>
            m.Send(It.IsAny<GetRelationshipsQuery>(), cancellationToken)
        ).ReturnsAsync(response);

        var result = await sut.GetRelationships(query.Ukprn, query.AccountLegalEntityId, cancellationToken);

        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().Be(response.Result);
    }

    [Test]
    [MoqAutoData]
    public async Task GetRelationships_HandlerReturnsData_ReturnsNotFoundResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] RelationshipsController sut,
        GetRelationshipsQuery query,
        GetRelationshipsQueryResult queryResult,
        CancellationToken cancellationToken
    )
    {
        var response = ValidatedResponse<GetRelationshipsQueryResult?>.EmptySuccessResponse();

        mediatorMock.Setup(m =>
            m.Send(It.IsAny<GetRelationshipsQuery>(), cancellationToken)
        ).ReturnsAsync(response);

        var result = await sut.GetRelationships(query.Ukprn, query.AccountLegalEntityId, cancellationToken);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    [MoqAutoData]
    public async Task GetRelationships_HandlerReturnsData_ReturnsBadRequestResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] RelationshipsController sut,
        GetRelationshipsQuery query,
        List<ValidationFailure> errors,
        CancellationToken cancellationToken
    )
    {
        var errorResponse = new ValidatedResponse<GetRelationshipsQueryResult?>(errors);

        mediatorMock.Setup(m =>
           m.Send(It.IsAny<GetRelationshipsQuery>(), cancellationToken)
        ).ReturnsAsync(errorResponse);

        var result = await sut.GetRelationships(0, query.AccountLegalEntityId, cancellationToken);

        result.Should().BeOfType<BadRequestObjectResult>();
    }
}
