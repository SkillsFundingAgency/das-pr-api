using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.PR.Api.Authorization;
using SFA.DAS.PR.Api.Common;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Application.Permissions.Queries.GetEmployerRelationships;

namespace SFA.DAS.PR.Api.Controllers;

[ApiController]
[Route("relationships")]
public class EmployerRelationshipsController(IMediator _mediator) : ActionResponseControllerBase
{
    public override string ControllerName => "EmployerRelationships";

    [HttpGet("employeraccount/{accountHashedId}")]
    [Authorize(Policy = Policies.Management)]
    [ProducesResponseType(typeof(GetEmployerRelationshipsQueryResult), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetEmployerRelationships(string accountHashedId, [FromQuery]long? ukprn, [FromQuery]string? accountlegalentityPublicHashedId, CancellationToken cancellationToken)
    {
        GetEmployerRelationshipsQuery query = new(accountHashedId, ukprn, accountlegalentityPublicHashedId);

        ValidatedResponse<GetEmployerRelationshipsQueryResult> result = await _mediator.Send(query, cancellationToken);

        return GetResponse(result);
    }
}
