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

    [Authorize(Policy = Policies.Management)]
    [HttpGet("employeraccount/{accountHashedId}")]
    [ProducesResponseType(typeof(GetEmployerRelationshipsQueryResult), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetEmployerRelationships(string accountHashedId, CancellationToken cancellationToken)
    {
        GetEmployerRelationshipsQuery query = new(accountHashedId);

        ValidatedResponse<GetEmployerRelationshipsQueryResult> result = await _mediator.Send(query, cancellationToken);

        return GetResponse(result);
    }
}