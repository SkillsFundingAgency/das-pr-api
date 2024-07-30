using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.PR.Api.Authorization;
using SFA.DAS.PR.Api.Common;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Application.Relationships.Queries.GetRelationships;

namespace SFA.DAS.PR.Api.Controllers;

[ApiController]
[Route("relationships")]
public class RelationshipsController(IMediator _mediator) : ActionResponseControllerBase
{
    public override string ControllerName => "Relationships";

    [HttpGet]
    [Authorize(Policy = Policies.Management)]
    [ProducesResponseType(typeof(GetRelationshipsQueryResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(List<ValidationError>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRelationships([FromQuery] long? ukprn, [FromQuery] long? accountLegalEntityId, CancellationToken cancellationToken)
    {
        GetRelationshipsQuery query = new(ukprn, accountLegalEntityId);

        ValidatedResponse<GetRelationshipsQueryResult?> result = await _mediator.Send(query, cancellationToken);

        return GetResponse(result);
    }
}
