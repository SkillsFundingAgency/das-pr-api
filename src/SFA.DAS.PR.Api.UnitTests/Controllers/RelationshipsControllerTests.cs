using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.PR.Api.Controllers;
using SFA.DAS.PR.Application.EmployerRelationships.Queries.GetProviderEmployerRelationship;
using SFA.DAS.PR.Application.Mediatr.Responses;
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
    public async Task GetProviderEmployerRelationship_InvokesQueryHandler(
       [Frozen] Mock<IMediator> mediatorMock,
       [Greedy] RelationshipsController sut,
       GetProviderEmployerRelationshipQuery query,
       CancellationToken cancellationToken
    )
    {
        await sut.GetProviderEmployerRelationship(query.Ukprn, query.AccountLegalEntityId, cancellationToken);

        mediatorMock.Verify(m =>
            m.Send(It.IsAny<GetProviderEmployerRelationshipQuery>(), cancellationToken)
        );
    }

    [Test]
    [MoqAutoData]
    public async Task GetProviderEmployerRelationship_HandlerReturnsData_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] RelationshipsController sut,
        GetProviderEmployerRelationshipQuery query,
        GetProviderEmployerRelationshipQueryResult queryResult,
        CancellationToken cancellationToken
    )
    {
        var response = new ValidatedResponse<GetProviderEmployerRelationshipQueryResult?>(queryResult);

        mediatorMock.Setup(m =>
            m.Send(It.IsAny<GetProviderEmployerRelationshipQuery>(), cancellationToken)
        ).ReturnsAsync(response);

        var result = await sut.GetProviderEmployerRelationship(query.Ukprn, query.AccountLegalEntityId, cancellationToken);

        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().Be(response.Result);
    }

    [Test]
    [MoqAutoData]
    public async Task GetProviderEmployerRelationship_HandlerReturnsData_ReturnsNotFoundResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] RelationshipsController sut,
        GetProviderEmployerRelationshipQuery query,
        GetProviderEmployerRelationshipQueryResult queryResult,
        CancellationToken cancellationToken
    )
    {
        var response = ValidatedResponse<GetProviderEmployerRelationshipQueryResult?>.EmptySuccessResponse();

        mediatorMock.Setup(m =>
            m.Send(It.IsAny<GetProviderEmployerRelationshipQuery>(), cancellationToken)
        ).ReturnsAsync(response);

        var result = await sut.GetProviderEmployerRelationship(query.Ukprn, query.AccountLegalEntityId, cancellationToken);

        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    [MoqAutoData]
    public async Task GetProviderEmployerRelationship_HandlerReturnsData_ReturnsBadRequestResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] RelationshipsController sut,
        GetProviderEmployerRelationshipQuery query,
        List<ValidationFailure> errors,
        CancellationToken cancellationToken
    )
    {
        var errorResponse = new ValidatedResponse<GetProviderEmployerRelationshipQueryResult?>(errors);

        mediatorMock.Setup(m =>
           m.Send(It.IsAny<GetProviderEmployerRelationshipQuery>(), cancellationToken)
        ).ReturnsAsync(errorResponse);

        var result = await sut.GetProviderEmployerRelationship(0, query.AccountLegalEntityId, cancellationToken);

        result.Should().BeOfType<BadRequestObjectResult>();
    }
}
