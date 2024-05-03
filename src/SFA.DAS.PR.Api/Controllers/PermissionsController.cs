using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.PR.Api.Common;
using SFA.DAS.PR.Api.RouteValues.Permissions;
using SFA.DAS.PR.Api.SwaggerExamples;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Application.Permissions.Queries.GetPermissionsHas;
using Swashbuckle.AspNetCore.Filters;

namespace SFA.DAS.PR.Api.Controllers;

[ApiController]
[Route("permissions")]
public class PermissionsController(IMediator _mediator) : ActionResponseControllerBase
{
    public override string ControllerName => "Permissions";

    [HttpGet("has")]
    [Authorize]
    [ProducesResponseType(typeof(bool), StatusCodes.Status400BadRequest)]
    [SwaggerRequestExample(typeof(HasPermissionRouteValues), typeof(HasPermissionRouteValuesExample))]

    public async Task<IActionResult> HasPermission([FromQuery] HasPermissionRouteValues routeValues, CancellationToken cancellationToken)
    {
        GetPermissionsHasQuery query = new GetPermissionsHasQuery
        {
            Ukprn = routeValues.Ukprn,
            PublicHashedId = routeValues.PublicHashedId,
            Operations = routeValues.Operations
        };

        ValidatedResponse<GetPermissionsHasResult> result = await _mediator.Send(query, cancellationToken);

        return GetResponse(result);
    }
}
