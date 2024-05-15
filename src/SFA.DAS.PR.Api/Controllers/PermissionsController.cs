using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.PR.Api.Authorization;
using SFA.DAS.PR.Api.Common;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Application.Permissions.Queries.GetPermissions;
using SFA.DAS.PR.Application.Permissions.Queries.HasRelationshipWithPermission;

namespace SFA.DAS.PR.Api.Controllers;

[ApiController]
[Route("permissions")]
public class PermissionsController(IMediator _mediator) : ActionResponseControllerBase
{
    public override string ControllerName => "AccountProviders";

    [HttpGet]
    [Authorize(Policy = Policies.Management)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPermissions([FromQuery] string accountHashedId, CancellationToken cancellationToken)
    {
        GetPermissionsQuery query = new(accountHashedId);

        ValidatedResponse<GetPermissionsQueryResult> result = await _mediator.Send(query, cancellationToken);

        return GetResponse(result);
    }

    [HttpGet]
    [Route("has-relationship-with")]
    [Authorize(Policy = Policies.Integration)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> HasRelationshipWithPermission([FromQuery] HasRelationshipWithPermissionQuery query, CancellationToken cancellationToken)
    {
        ValidatedBooleanResult result = await _mediator.Send(query, cancellationToken);

        return GetBooleanResponse(result);
    }
}