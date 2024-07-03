using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.PR.Api.Controllers;
using SFA.DAS.PR.Application.EmployerRelationships.Queries.GetProviderEmployerRelationship;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Application.Permissions.Queries.GetEmployerRelationships;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Api.UnitTests.Controllers;

public class EmployerRelationshipsControllerTests
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
    public async Task GetEmployerRelationships_InvokesQueryHandler(
       [Frozen] Mock<IMediator> mediatorMock,
       [Greedy] EmployerRelationshipsController sut,
       GetEmployerRelationshipsQuery query,
       CancellationToken cancellationToken
    )
    {
        await sut.GetEmployerRelationships(query.AccountHashedId, cancellationToken);

        mediatorMock.Verify(m =>
            m.Send(It.IsAny<GetEmployerRelationshipsQuery>(), cancellationToken)
        );
    }

    [Test]
    [MoqAutoData]
    public async Task GetAllPermissionsForAccount_HandlerReturnsData_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] EmployerRelationshipsController sut,
        GetEmployerRelationshipsQuery query,
        CancellationToken cancellationToken
    )
    {
        var account = _fixture.Create<Account>();

        var permissionsList = account.AccountLegalEntities
            .Select(a => (AccountLegalEntityPermissionsModel)a)
            .ToList();

        var queryResult = new GetEmployerRelationshipsQueryResult(permissionsList);

        var response = new ValidatedResponse<GetEmployerRelationshipsQueryResult>(queryResult);

        mediatorMock.Setup(m =>
            m.Send(It.IsAny<GetEmployerRelationshipsQuery>(), cancellationToken)
        ).ReturnsAsync(response);

        var result = await sut.GetEmployerRelationships(query.AccountHashedId, cancellationToken);

        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().Be(response.Result);
    }

    [Test]
    [MoqAutoData]
    public async Task GetProviderEmployerRelationship_InvokesQueryHandler(
       [Frozen] Mock<IMediator> mediatorMock,
       [Greedy] EmployerRelationshipsController sut,
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
        [Greedy] EmployerRelationshipsController sut,
        GetProviderEmployerRelationshipQuery query,
        GetProviderEmployerRelationshipQueryResult queryResult,
        CancellationToken cancellationToken
    )
    {
        var response = new ValidatedResponse<GetProviderEmployerRelationshipQueryResult>(queryResult);

        mediatorMock.Setup(m =>
            m.Send(It.IsAny<GetProviderEmployerRelationshipQuery>(), cancellationToken)
        ).ReturnsAsync(response);

        var result = await sut.GetProviderEmployerRelationship(query.Ukprn, query.AccountLegalEntityId, cancellationToken);

        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().Be(response.Result);
    }
}
