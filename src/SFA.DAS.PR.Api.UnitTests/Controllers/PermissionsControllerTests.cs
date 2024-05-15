using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.PR.Api.Common;
using SFA.DAS.PR.Api.Controllers;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Application.Permissions.Queries.GetPermissions;
using SFA.DAS.PR.Application.Permissions.Queries.HasRelationshipWithPermission;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Api.UnitTests.Controllers;

public class PermissionsControllerTests
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
    public async Task HasRelationshipWithPermission_InvokesQueryHandler(
       [Frozen] Mock<IMediator> mediatorMock,
       [Greedy] PermissionsController sut,
       long ukprn,
       Operation operation,
       CancellationToken cancellationToken
    )
    {
        HasRelationshipWithPermissionQuery query = new()
        {
            Ukprn = ukprn,
            Operation = operation,
        };

        await sut.HasRelationshipWithPermission(query, cancellationToken);

        mediatorMock.Verify(m =>
            m.Send(query, cancellationToken)
        );
    }

    [Test]
    [MoqAutoData]
    public async Task HasRelationshipWithPermission_HandlerReturnsNullResult_ReturnsNotFoundResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] PermissionsController sut,
        long ukprn,
        Operation operation,
        CancellationToken cancellationToken
    )
    {
        HasRelationshipWithPermissionQuery query = new()
        {
            Ukprn = ukprn,
            Operation = operation,
        };

        var notFoundResponse = ValidatedBooleanResult.EmptySuccessResponse();

        mediatorMock.Setup(m =>
            m.Send(query, cancellationToken)
        ).ReturnsAsync(notFoundResponse);

        var result = await sut.HasRelationshipWithPermission(query, cancellationToken);
        result.As<NotFoundResult>().Should().NotBeNull();
    }

    [Test]
    [MoqAutoData]
    public async Task HasRelationshipWithPermission_HandlerReturnsData_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] PermissionsController sut,
        long ukprn,
        Operation operation,
        CancellationToken cancellationToken
    )
    {
        HasRelationshipWithPermissionQuery query = new()
        {
            Ukprn = ukprn,
            Operation = operation,
        };

        var response = new ValidatedBooleanResult(true);

        mediatorMock.Setup(m =>
            m.Send(query, cancellationToken)
        ).ReturnsAsync(response);

        var result = await sut.HasRelationshipWithPermission(query, cancellationToken);

        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().Be(response.Result);
    }

    [Test]
    [MoqAutoData]
    public async Task HasRelationshipWithPermission_InvalidRequest_ReturnsBadRequestResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] PermissionsController sut,
        List<ValidationFailure> errors,
        CancellationToken cancellationToken
    )
    {
        HasRelationshipWithPermissionQuery query = new()
        {
            Ukprn = null,
            Operation = null,
        };

        var errorResponse = new ValidatedBooleanResult(errors);

        mediatorMock.Setup(m =>
            m.Send(query, cancellationToken)
        ).ReturnsAsync(errorResponse);

        var result = await sut.HasRelationshipWithPermission(query, cancellationToken);
        result.As<BadRequestObjectResult>().Should().NotBeNull();
        result.As<BadRequestObjectResult>().Value.As<List<ValidationError>>().Count.Should().Be(errors.Count);
    }

    [Test]
    [MoqAutoData]
    public async Task GetPermissions_InvokesQueryHandler(
       [Frozen] Mock<IMediator> mediatorMock,
       [Greedy] PermissionsController sut,
       GetPermissionsQuery query,
       CancellationToken cancellationToken
    )
    {
        await sut.GetPermissions(query.AccountHashedId, cancellationToken);

        mediatorMock.Verify(m =>
            m.Send(It.IsAny<GetPermissionsQuery>(), cancellationToken)
        );
    }

    [Test]
    [MoqAutoData]
    public async Task GetPermissions_HandlerReturnsData_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] PermissionsController sut,
        GetPermissionsQuery query,
        CancellationToken cancellationToken
    )
    {
        var account = _fixture.Create<Account>();

        var permissionsList = account.AccountLegalEntities
            .Select(a => (AccountLegalEntityPermissionsModel)a)
            .ToList();

        var queryResult = new GetPermissionsQueryResult(permissionsList);

        var response = new ValidatedResponse<GetPermissionsQueryResult>(queryResult);

        mediatorMock.Setup(m =>
            m.Send(It.IsAny<GetPermissionsQuery>(), cancellationToken)
        ).ReturnsAsync(response);

        var result = await sut.GetPermissions(query.AccountHashedId, cancellationToken);

        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().Be(response.Result);
    }
}
