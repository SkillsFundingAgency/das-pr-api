﻿using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.PR.Api.Controllers;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Application.Permissions.Commands.PostPermissions;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Api.UnitTests.Controllers.Permissions;

public class PermissionsControllerPostTests
{
    [Test]
    [MoqAutoData]
    public async Task PostPermission_InvokesQueryHandler(
      [Frozen] Mock<IMediator> mediatorMock,
      [Greedy] PermissionsController sut,
      PostPermissionsCommand command,
      CancellationToken cancellationToken
   )
    {
        await sut.PostPermission(command, cancellationToken);

        mediatorMock.Verify(m =>
            m.Send(It.Is<PostPermissionsCommand>(q =>
                q.AccountLegalEntityId == command.AccountLegalEntityId && 
                q.Ukprn == command.Ukprn &&
                q.Operations == command.Operations &&
                q.UserRef == command.UserRef
            ),
            cancellationToken)
        );
    }

    [Test, MoqAutoData]
    public async Task PostPermission_HandlerReturnsData_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] PermissionsController sut,
        PostPermissionsCommand command,
        PostPermissionsCommandResult postPermissionsCommandResult,
        CancellationToken cancellationToken
    )
    {
        var response = new ValidatedResponse<PostPermissionsCommandResult>(postPermissionsCommandResult);

        mediatorMock.Setup(m => m.Send(
            It.Is<PostPermissionsCommand>(q =>
                q.AccountLegalEntityId == command.AccountLegalEntityId &&
                q.Ukprn == command.Ukprn &&
                q.Operations == command.Operations &&
                q.UserRef == command.UserRef
            ),
            cancellationToken)
        ).ReturnsAsync(response);

        var result = await sut.PostPermission(command, cancellationToken);
        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().Be(postPermissionsCommandResult);
    }
}
