﻿using AutoFixture;
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
using SFA.DAS.PR.Application.Requests.Commands.AcceptAddAccountRequest;
using SFA.DAS.PR.Application.Requests.Commands.AcceptCreateAccountRequest;
using SFA.DAS.PR.Application.Requests.Commands.AcceptPermissionsRequest;
using SFA.DAS.PR.Application.Requests.Commands.CreateAddAccountRequest;
using SFA.DAS.PR.Application.Requests.Commands.CreateNewAccountRequest;
using SFA.DAS.PR.Application.Requests.Commands.CreatePermissionRequest;
using SFA.DAS.PR.Application.Requests.Commands.DeclinedRequest;
using SFA.DAS.PR.Application.Requests.Queries.GetRequest;
using SFA.DAS.PR.Application.Requests.Queries.LookupRequests;
using SFA.DAS.PR.Domain.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Api.UnitTests.Controllers;

public class RequestsControllerTests
{
    private Fixture _fixture = null!;
    private const string Paye = "AAA/222";

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Test]
    [MoqAutoData]
    public async Task CreateAddAccountRequest_InvokesQueryHandler(
       [Frozen] Mock<IMediator> mediatorMock,
       [Greedy] RequestsController sut,
       CreateAddAccountRequestCommand command,
       CancellationToken cancellationToken
    )
    {
        await sut.CreateAddAccountRequest(command, cancellationToken);

        mediatorMock.Verify(m =>
            m.Send(It.IsAny<CreateAddAccountRequestCommand>(), It.IsAny<CancellationToken>())
        );
    }

    [Test]
    [MoqAutoData]
    public async Task CreateAddAccountRequest_InvalidRequest_ReturnsBadRequestResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] RequestsController sut,
        List<ValidationFailure> errors,
        CancellationToken cancellationToken
    )
    {
        var errorResponse = new ValidatedResponse<CreateAddAccountRequestCommandResult>(errors);

        mediatorMock.Setup(m =>
            m.Send(
                It.IsAny<CreateAddAccountRequestCommand>(),
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(errorResponse);

        var result = await sut.CreateAddAccountRequest(
            new CreateAddAccountRequestCommand()
            {
                AccountLegalEntityId = 1,
                Ukprn = 1,
                Operations = [],
                Paye = Paye,
                RequestedBy = Guid.NewGuid().ToString()
            },
            cancellationToken
        );
        result.As<BadRequestObjectResult>().Should().NotBeNull();
        result.As<BadRequestObjectResult>().Value.As<List<ValidationError>>().Count.Should().Be(errors.Count);
    }

    [Test]
    [MoqAutoData]
    public async Task CreatePermissionRequest_InvokesQueryHandler(
       [Frozen] Mock<IMediator> mediatorMock,
       [Greedy] RequestsController sut,
       CreatePermissionRequestCommand command,
       CancellationToken cancellationToken
    )
    {
        await sut.CreatePermissionRequest(command, cancellationToken);

        mediatorMock.Verify(m =>
            m.Send(It.IsAny<CreatePermissionRequestCommand>(), It.IsAny<CancellationToken>())
        );
    }

    [Test]
    [MoqAutoData]
    public async Task CreatePermissionRequest_InvalidRequest_ReturnsBadRequestResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] RequestsController sut,
        List<ValidationFailure> errors,
        CancellationToken cancellationToken
    )
    {
        var errorResponse = new ValidatedResponse<CreatePermissionRequestCommandResult>(errors);

        mediatorMock.Setup(m =>
            m.Send(
                It.IsAny<CreatePermissionRequestCommand>(),
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(errorResponse);

        var result = await sut.CreatePermissionRequest(
            new CreatePermissionRequestCommand()
            {
                AccountLegalEntityId = 1,
                Ukprn = 1,
                Operations = [],
                RequestedBy = Guid.NewGuid().ToString()
            },
            cancellationToken
        );
        result.As<BadRequestObjectResult>().Should().NotBeNull();
        result.As<BadRequestObjectResult>().Value.As<List<ValidationError>>().Count.Should().Be(errors.Count);
    }

    [Test]
    [MoqAutoData]
    public async Task GetRequest_InvokesQueryHandler(
       [Frozen] Mock<IMediator> mediatorMock,
       [Greedy] RequestsController sut,
       Guid id,
       CancellationToken cancellationToken
    )
    {
        await sut.GetRequest(id, cancellationToken);

        mediatorMock.Verify(m =>
            m.Send(It.Is<GetRequestQuery>(a => a.RequestId == id), It.IsAny<CancellationToken>())
        );
    }

    [Test]
    [MoqAutoData]
    public async Task GetRequest_Returns_NotFoundResult(
       [Frozen] Mock<IMediator> mediatorMock,
       [Greedy] RequestsController sut,
       GetRequestQuery query,
       CancellationToken cancellationToken
    )
    {
        var result = await sut.GetRequest(query.RequestId, cancellationToken);

        mediatorMock.Setup(m =>
            m.Send(
                It.Is<GetRequestQuery>(a => a.RequestId == query.RequestId),
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(new ValidatedResponse<RequestModel?>());

        result.Should().BeOfType<NotFoundResult>().And.NotBeNull();
    }

    [Test]
    [MoqAutoData]
    public async Task LookupRequests_InvokesQueryHandler(
       [Frozen] Mock<IMediator> mediatorMock,
       [Greedy] RequestsController sut,
       LookupRequestsQuery query,
       CancellationToken cancellationToken
    )
    {
        await sut.LookupRequests(query.Ukprn!.Value, query.Paye, query.Email, query.AccountLegalEntityId, cancellationToken);

        mediatorMock.Verify(m =>
            m.Send(It.Is<LookupRequestsQuery>(a => a.Ukprn == query.Ukprn && a.Paye == query.Paye), It.IsAny<CancellationToken>())
        );
    }

    [Test]
    [MoqAutoData]
    public async Task LookupRequests_Returns_NotFoundResult(
       [Frozen] Mock<IMediator> mediatorMock,
       [Greedy] RequestsController sut,
       LookupRequestsQuery query,
       CancellationToken cancellationToken
    )
    {
        var result = await sut.LookupRequests(query.Ukprn!.Value, query.Paye, query.Email, query.AccountLegalEntityId, cancellationToken);

        mediatorMock.Setup(m =>
            m.Send(
                It.Is<LookupRequestsQuery>(a => a.Ukprn == query.Ukprn && a.Paye == query.Paye),
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(new ValidatedResponse<RequestModel?>());

        result.Should().BeOfType<NotFoundResult>().And.NotBeNull();
    }

    [Test]
    [MoqAutoData]
    public async Task LookupRequests_InvalidRequest_ReturnsBadRequestResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] RequestsController sut,
        List<ValidationFailure> errors,
        CancellationToken cancellationToken
    )
    {
        var errorResponse = new ValidatedResponse<RequestModel?>(errors);

        mediatorMock.Setup(m =>
            m.Send(
                It.IsAny<LookupRequestsQuery>(),
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(errorResponse);

        var result = await sut.LookupRequests(
            10000001,
            "PAYE",
            null,
            null,
            cancellationToken
        );

        result.As<BadRequestObjectResult>().Should().NotBeNull();
        result.As<BadRequestObjectResult>().Value.As<List<ValidationError>>().Count.Should().Be(errors.Count);
    }

    [Test]
    [MoqAutoData]
    public async Task CreateNewAccountRequest_InvokesQueryHandler(
       [Frozen] Mock<IMediator> mediatorMock,
       [Greedy] RequestsController sut,
       CreateNewAccountRequestCommand command,
       CancellationToken cancellationToken
    )
    {
        await sut.CreateNewAccountRequest(command, cancellationToken);

        mediatorMock.Verify(m =>
            m.Send(It.Is<CreateNewAccountRequestCommand>(a =>
                a.Ukprn == command.Ukprn &&
                a.EmployerPAYE == command.EmployerPAYE
            ), It.IsAny<CancellationToken>())
        );
    }

    [Test]
    [MoqAutoData]
    public async Task AcceptCreateAccountRequest_InvokesQueryHandler(
       [Frozen] Mock<IMediator> mediatorMock,
       [Greedy] RequestsController sut,
       AcceptCreateAccountRequestModel model,
       Guid requestId,
       CancellationToken cancellationToken
    )
    {
        await sut.AcceptCreateAccountRequest(requestId, model, cancellationToken);

        mediatorMock.Verify(m =>
            m.Send(It.Is<AcceptCreateAccountRequestCommand>(a =>
                a.RequestId == requestId &&
                a.AccountLegalEntity.Id == model.AccountLegalEntityDetails.Id &&
                a.Account.Id == model.AccountDetails.Id
            ), It.IsAny<CancellationToken>())
        );
    }

    [Test]
    [MoqAutoData]
    public async Task DeclinedRequest_InvokesQueryHandler(
       [Frozen] Mock<IMediator> mediatorMock,
       [Greedy] RequestsController sut,
       DeclinedRequestModel model,
       Guid requestId,
       CancellationToken cancellationToken
    )
    {
        await sut.DeclineRequest(requestId, model, cancellationToken);

        mediatorMock.Verify(m =>
            m.Send(It.Is<DeclinedRequestCommand>(a =>
                a.RequestId == requestId &&
                a.ActionedBy == model.ActionedBy
            ), It.IsAny<CancellationToken>())
        );
    }

    [Test]
    [MoqAutoData]
    public async Task DeclinedRequest_ReturnsBadRequestResponse(
       [Frozen] Mock<IMediator> mediatorMock,
       [Greedy] RequestsController sut,
       DeclinedRequestModel model,
       Guid requestId,
       List<ValidationFailure> errors,
       CancellationToken cancellationToken
    )
    {
        var errorResponse = new ValidatedResponse<Unit>(errors);

        mediatorMock.Setup(m =>
            m.Send(
                It.Is<DeclinedRequestCommand>(a =>
                    a.ActionedBy == model.ActionedBy &&
                    a.RequestId == requestId
                ),
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(errorResponse);

        var result = await sut.DeclineRequest(requestId, model, cancellationToken);

        result.As<BadRequestObjectResult>().Should().NotBeNull();
        result.As<BadRequestObjectResult>().Value.As<List<ValidationError>>().Count.Should().Be(errors.Count);
    }

    [Test]
    [MoqAutoData]
    public async Task AcceptPermissionsRequest_InvokesQueryHandler(
       [Frozen] Mock<IMediator> mediatorMock,
       [Greedy] RequestsController sut,
       AcceptPermissionsRequestModel model,
       Guid requestId,
       CancellationToken cancellationToken
    )
    {
        await sut.AcceptPermissionsRequest(requestId, model, cancellationToken);

        mediatorMock.Verify(m =>
            m.Send(It.Is<AcceptPermissionsRequestCommand>(a =>
                a.RequestId == requestId &&
                a.ActionedBy == model.ActionedBy
            ), It.IsAny<CancellationToken>())
        );
    }

    [Test]
    [MoqAutoData]
    public async Task AcceptPermissionsRequest_ReturnsBadRequestResponse(
       [Frozen] Mock<IMediator> mediatorMock,
       [Greedy] RequestsController sut,
       AcceptPermissionsRequestModel model,
       Guid requestId,
       List<ValidationFailure> errors,
       CancellationToken cancellationToken
    )
    {
        var errorResponse = new ValidatedResponse<Unit>(errors);

        mediatorMock.Setup(m =>
            m.Send(
                It.Is<AcceptPermissionsRequestCommand>(a =>
                    a.ActionedBy == model.ActionedBy &&
                    a.RequestId == requestId
                ),
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(errorResponse);

        var result = await sut.AcceptPermissionsRequest(requestId, model, cancellationToken);

        result.As<BadRequestObjectResult>().Should().NotBeNull();
        result.As<BadRequestObjectResult>().Value.As<List<ValidationError>>().Count.Should().Be(errors.Count);
    }

    [Test]
    [MoqAutoData]
    public async Task AcceptAddAccountRequest_InvokesQueryHandler(
       [Frozen] Mock<IMediator> mediatorMock,
       [Greedy] RequestsController sut,
       AcceptAddAccountRequestModel model,
       Guid requestId,
       CancellationToken cancellationToken
    )
    {
        await sut.AcceptAddAccountRequest(requestId, model, cancellationToken);

        mediatorMock.Verify(m =>
            m.Send(It.Is<AcceptAddAccountRequestCommand>(a =>
                a.RequestId == requestId &&
                a.ActionedBy == model.ActionedBy
            ), It.IsAny<CancellationToken>())
        );
    }

    [Test]
    [MoqAutoData]
    public async Task AcceptAddAccountRequest_ReturnsBadRequestResponse(
       [Frozen] Mock<IMediator> mediatorMock,
       [Greedy] RequestsController sut,
       AcceptAddAccountRequestModel model,
       Guid requestId,
       List<ValidationFailure> errors,
       CancellationToken cancellationToken
    )
    {
        var errorResponse = new ValidatedResponse<Unit>(errors);

        mediatorMock.Setup(m =>
            m.Send(
                It.Is<AcceptAddAccountRequestCommand>(a =>
                    a.ActionedBy == model.ActionedBy &&
                    a.RequestId == requestId
                ),
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(errorResponse);

        var result = await sut.AcceptAddAccountRequest(requestId, model, cancellationToken);

        result.As<BadRequestObjectResult>().Should().NotBeNull();
        result.As<BadRequestObjectResult>().Value.As<List<ValidationError>>().Count.Should().Be(errors.Count);
    }
}
