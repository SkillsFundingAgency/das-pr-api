using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.PR.Api.Authorization;
using SFA.DAS.PR.Api.Common;
using SFA.DAS.PR.Api.SwaggerExamples;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Application.Permissions.Commands.PostPermissions;
using SFA.DAS.PR.Application.Permissions.Commands.RemovePermissions;
using SFA.DAS.PR.Application.Permissions.Queries.GetHasPermissions;
using SFA.DAS.PR.Application.Permissions.Queries.GetPermissions;
using SFA.DAS.PR.Application.Permissions.Queries.HasRelationshipWithPermission;
using Swashbuckle.AspNetCore.Filters;

namespace SFA.DAS.PR.Api.Controllers;

[ApiController]
[Route("permissions")]
public class PermissionsController(IMediator _mediator) : ActionResponseControllerBase
{
    public override string ControllerName => "Permissions";

    [HttpGet]
    [Route("has-relationship-with")]
    [Authorize(Policy = Policies.Integration)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(List<ValidationError>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> HasRelationshipWithPermission([FromQuery] HasRelationshipWithPermissionQuery query, CancellationToken cancellationToken)
    {
        ValidatedResponse<bool> result = await _mediator.Send(query, cancellationToken);
        return GetResponse(result);
    }

    [HttpGet]
    [Authorize(Policy = Policies.Management)]
    [ProducesResponseType(typeof(GetPermissionsQueryResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(List<ValidationError>), StatusCodes.Status400BadRequest)]
    [SwaggerRequestExample(typeof(GetPermissionsQueryResult), typeof(GetPermissionsQueryResultExample))]
    public async Task<IActionResult> GetPermissions([FromQuery] GetPermissionsQuery query, CancellationToken cancellationToken)
    {
        ValidatedResponse<GetPermissionsQueryResult?> result = await _mediator.Send(query, cancellationToken);
        return GetResponse(result);
    }

    [HttpGet("has")]
    [Authorize(Policy = Policies.Integration)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(List<ValidationError>), StatusCodes.Status400BadRequest)]
    [SwaggerRequestExample(typeof(GetHasPermissionsQuery), typeof(GetHasPermissionsQueryExample))]

    public async Task<IActionResult> HasPermission([FromQuery] GetHasPermissionsQuery query, CancellationToken cancellationToken)
    {
        ValidatedResponse<bool> result = await _mediator.Send(query, cancellationToken);
        return GetResponse(result);
    }

    [HttpPost]
    [Authorize(Policy = Policies.Management)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(List<ValidationError>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> PostPermission([FromBody] PostPermissionsCommand command, CancellationToken cancellationToken)
    {
        ValidatedResponse<PostPermissionsCommandResult> result = await _mediator.Send(command, cancellationToken);
        return GetResponse(result);
    }

    [HttpDelete]
    [Authorize(Policy = Policies.Management)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(List<ValidationError>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RemovePermission([FromQuery] RemovePermissionsCommand request, CancellationToken cancellationToken)
    {
        var response = await _mediator.Send(request, cancellationToken);
        return GetDeleteResponse(response);
    }
}
