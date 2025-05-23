﻿using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.PR.Api.Common;
using SFA.DAS.PR.Api.Controllers;
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
        await sut.GetEmployerRelationships(query.AccountId, cancellationToken);

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

        var result = await sut.GetEmployerRelationships(query.AccountId, cancellationToken);

        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().Be(response.Result);
    }

    [Test]
    [MoqAutoData]
    public async Task GetAllPermissionsForAccount_ReturnsBadRequestResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] EmployerRelationshipsController sut,
        GetEmployerRelationshipsQuery query,
        List<ValidationFailure> errors,
        CancellationToken cancellationToken)
    {
        mediatorMock.Setup(m => m.Send(It.IsAny<GetEmployerRelationshipsQuery>(), cancellationToken)).ReturnsAsync(new ValidatedResponse<GetEmployerRelationshipsQueryResult>(errors));

        var result = await sut.GetEmployerRelationships(query.AccountId, cancellationToken);

        result.As<BadRequestObjectResult>().Value.As<List<ValidationError>>().Count.Should().Be(errors.Count);
    }
}
