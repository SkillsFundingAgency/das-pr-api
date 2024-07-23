using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.PR.Api.Authorization;
using SFA.DAS.PR.Api.Common;
using SFA.DAS.PR.Application.Requests.Commands.CreateAddAccountRequest;

namespace SFA.DAS.PR.Api.Controllers;

[ApiController]
[Route("requests")]
public class RequestsController(IMediator _mediator) : ActionResponseControllerBase
{
    public override string ControllerName => "Requests";

    [HttpPost("addaccount")]
    [Authorize(Policy = Policies.Management)]
    [ProducesResponseType(typeof(CreateAddAccountRequestCommandResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateAddAccountRequest([FromBody]CreateAddAccountRequestCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command);
        return GetResponse(result);
    }
}
