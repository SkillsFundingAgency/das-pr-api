using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.PR.Api.Authorization;
using SFA.DAS.PR.Api.Common;
using SFA.DAS.PR.Api.SwaggerExamples;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Application.Permissions.Queries.GetAllPermissionsForAccount;
using SFA.DAS.PR.Application.Permissions.Queries.GetPermissionsForProviderOnAccount;
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
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> HasRelationshipWithPermission([FromQuery] HasRelationshipWithPermissionQuery query, CancellationToken cancellationToken)
    {
        ValidatedBooleanResult result = await _mediator.Send(query, cancellationToken);

        return GetBooleanResponse(result);
    }

    [HttpGet("account-permissions")]
    [Authorize(Policy = Policies.Management)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllPermissionsForAccount([FromQuery] string accountHashedId, CancellationToken cancellationToken)
    {
        GetAllPermissionsForAccountQuery query = new(accountHashedId);

        ValidatedResponse<GetAllPermissionsForAccountQueryResult> result = await _mediator.Send(query, cancellationToken);

        return GetResponse(result);
    }

    [HttpGet]
    [Authorize(Policy = Policies.Management)]
    [Authorize(Policy = Policies.Integration)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(GetPermissionsForProviderOnAccountQueryResult), StatusCodes.Status200OK)]
    [SwaggerRequestExample(typeof(GetPermissionsForProviderOnAccountQueryResult), typeof(GetPermissionsForProviderOnAccountQueryResultExample))]
    public async Task<IActionResult> GetPermissionsForProviderOnAccount([FromQuery] GetPermissionsForProviderOnAccountQuery query, CancellationToken cancellationToken)
    {
        ValidatedResponse<GetPermissionsForProviderOnAccountQueryResult> result = await _mediator.Send(query, cancellationToken);

        return GetResponse(result);
    }
}