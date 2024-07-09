using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.PR.Api.Authorization;
using SFA.DAS.PR.Api.Common;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Application.Notifications.Commands;

namespace SFA.DAS.PR.Api.Controllers;

[ApiController]
[Route("notifications")]
public class NotificationsController(IMediator _mediator) : ActionResponseControllerBase
{
    public override string ControllerName => "Notifications";

    [HttpPost]
    [Authorize(Policy = Policies.Management)]
    [ProducesResponseType(typeof(Unit), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(List<ValidationError>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostNotifications([FromBody] PostNotificationsCommand command, CancellationToken cancellationToken)
    {
        ValidatedResponse<Unit> result = await _mediator.Send(command, cancellationToken);
        return GetResponse(result);
    }
}
