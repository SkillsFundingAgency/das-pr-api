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
using SFA.DAS.PR.Application.Requests.Commands.CreateAddAccountRequest;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Api.UnitTests.Controllers;

public class RequestsControllerTests
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
                RequestedBy = Guid.NewGuid().ToString() 
            }, 
            cancellationToken
        );
        result.As<BadRequestObjectResult>().Should().NotBeNull();
        result.As<BadRequestObjectResult>().Value.As<List<ValidationError>>().Count.Should().Be(errors.Count);
    }
}