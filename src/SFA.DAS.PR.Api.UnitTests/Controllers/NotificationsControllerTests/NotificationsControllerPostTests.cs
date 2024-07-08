using AutoFixture.NUnit3;
using FluentAssertions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.PR.Api.Common;
using SFA.DAS.PR.Api.Controllers;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Application.Notifications.Commands;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Api.UnitTests.Controllers.NotificationsControllerTests;

public class NotificationsControllerPostTests
{
    [Test]
    [MoqAutoData]
    public async Task PostNotifications_InvokesQueryHandler(
      [Frozen] Mock<IMediator> mediatorMock,
      [Greedy] NotificationsController sut,
      PostNotificationsCommand command,
      CancellationToken cancellationToken
   )
    {
        await sut.PostNotifications(command, cancellationToken);

        mediatorMock.Verify(m =>
            m.Send(It.IsAny<PostNotificationsCommand>(), cancellationToken)
        );
    }

    [Test]
    [MoqAutoData]
    public async Task PostNotifications_HandlerReturnsData_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] NotificationsController sut,
        PostNotificationsCommand command,
        PostNotificationsCommandResult postNotificationsCommandResult,
        CancellationToken cancellationToken
    )
    {
        var response = new ValidatedResponse<PostNotificationsCommandResult>(postNotificationsCommandResult);

        mediatorMock.Setup(m => m.Send(
            It.IsAny<PostNotificationsCommand>(),
            cancellationToken)
        ).ReturnsAsync(response);

        var result = await sut.PostNotifications(command, cancellationToken);
        result.As<CreatedAtActionResult>().Should().NotBeNull();
        result.As<CreatedAtActionResult>().Value.Should().Be(postNotificationsCommandResult);
    }

    [Test]
    [MoqAutoData]
    public async Task PostNotifications_HandlerReturnsData_ReturnsBadRequest(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] NotificationsController sut,
        PostNotificationsCommand command,
        IList<ValidationFailure> validationErrors,
        CancellationToken cancellationToken
    )
    {
        var response = new ValidatedResponse<PostNotificationsCommandResult>(validationErrors);

        mediatorMock.Setup(m => m.Send(
            It.IsAny<PostNotificationsCommand>(),
            cancellationToken)
        ).ReturnsAsync(response);

        var result = await sut.PostNotifications(command, cancellationToken);
        result.As<BadRequestObjectResult>().Should().NotBeNull();
        result.As<BadRequestObjectResult>().Value.As<List<ValidationError>>().Count.Should().Be(validationErrors.Count);
    }
}
