using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.PR.Api.Authorization;
using SFA.DAS.PR.Api.Common;
using SFA.DAS.PR.Api.Models;
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

namespace SFA.DAS.PR.Api.Controllers;

[ApiController]
[Route("requests")]
public class RequestsController(IMediator _mediator) : ActionResponseControllerBase
{
    public override string ControllerName => "Requests";

    [HttpPost("addaccount")]
    [Authorize(Policy = Policies.Management)]
    [ProducesResponseType(typeof(CreateAddAccountRequestCommandResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(List<ValidationError>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateAddAccountRequest([FromBody] CreateAddAccountRequestCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return GetResponse(result);
    }

    [HttpPost("createaccount")]
    [Authorize(Policy = Policies.Management)]
    [ProducesResponseType(typeof(CreateNewAccountRequestCommandResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(List<ValidationError>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateNewAccountRequest([FromBody] CreateNewAccountRequestCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return GetResponse(result);
    }

    [HttpPost("permission")]
    [Authorize(Policy = Policies.Management)]
    [ProducesResponseType(typeof(CreatePermissionRequestCommandResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(List<ValidationError>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreatePermissionRequest([FromBody] CreatePermissionRequestCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return GetResponse(result);
    }

    [HttpGet("{requestId}")]
    [Authorize(Policy = Policies.Management)]
    [ProducesResponseType(typeof(RequestModel), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRequest([FromRoute] Guid requestId, CancellationToken cancellationToken)
    {
        GetRequestQuery query = new(requestId);
        var result = await _mediator.Send(query, cancellationToken);
        return GetResponse(result);
    }

    [HttpGet]
    [Authorize(Policy = Policies.Management)]
    [ProducesResponseType(typeof(RequestModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(List<ValidationError>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> LookupRequests([FromQuery] long ukprn, [FromQuery] string? paye, [FromQuery] string? email, CancellationToken cancellationToken)
    {
        LookupRequestsQuery query = new(ukprn, paye, email);
        var result = await _mediator.Send(query, cancellationToken);
        return GetResponse(result);
    }

    [HttpPost("{requestId:guid}/createaccount/accepted")]
    [Authorize(Policy = Policies.Management)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(List<ValidationError>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AcceptCreateAccountRequest([FromRoute] Guid requestId, [FromBody] AcceptCreateAccountRequestModel model, CancellationToken cancellationToken)
    {
        AcceptCreateAccountRequestCommand command = new()
        {
            RequestId = requestId,
            Account = model.AccountDetails,
            AccountLegalEntity = model.AccountLegalEntityDetails,
            ActionedBy = model.ActionedBy,
        };

        var result = await _mediator.Send(command, cancellationToken);
        return GetResponse(result);
    }

    [HttpPost("{requestId:guid}/permission/accepted")]
    [Authorize(Policy = Policies.Management)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(List<ValidationError>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AcceptPermissionsRequest([FromRoute] Guid requestId, [FromBody] AcceptPermissionsRequestModel model, CancellationToken cancellationToken)
    {
        AcceptPermissionsRequestCommand command = new()
        {
            RequestId = requestId,
            ActionedBy = model.ActionedBy
        };

        var result = await _mediator.Send(command, cancellationToken);
        return GetResponse(result);
    }

    [HttpPut("{requestId:guid}/declined")]
    [Authorize(Policy = Policies.Management)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(List<ValidationError>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeclineRequest([FromRoute] Guid requestId, [FromBody] DeclinedRequestModel model, CancellationToken cancellationToken)
    {
        DeclinedRequestCommand command = new DeclinedRequestCommand()
        {
            RequestId = requestId,
            ActionedBy = model.ActionedBy
        };

        var result = await _mediator.Send(command, cancellationToken);
        return GetResponse(result);
    }

    [HttpPost("{requestId:guid}/addaccount/accepted")]
    [Authorize(Policy = Policies.Management)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(List<ValidationError>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AcceptAddAccountRequest([FromRoute] Guid requestId, [FromBody] AcceptAddAccountRequestModel model, CancellationToken cancellationToken)
    {
        AcceptAddAccountRequestCommand command = new AcceptAddAccountRequestCommand()
        {
            RequestId = requestId,
            ActionedBy = model.ActionedBy
        };

        var result = await _mediator.Send(command, cancellationToken);
        return GetResponse(result);
    }
}
