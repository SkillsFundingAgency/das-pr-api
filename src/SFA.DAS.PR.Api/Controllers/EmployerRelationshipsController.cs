using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.PR.Api.Attributes;
using SFA.DAS.PR.Api.Authorization;
using SFA.DAS.PR.Api.Common;
using SFA.DAS.PR.Application.EmployerRelationships.Queries.GetProviderEmployerRelationship;
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
    public async Task<IActionResult> GetEmployerRelationships(string accountHashedId, CancellationToken cancellationToken)
    {
        GetEmployerRelationshipsQuery query = new(accountHashedId);

        ValidatedResponse<GetEmployerRelationshipsQueryResult> result = await _mediator.Send(query, cancellationToken);

        return GetResponse(result);
    }

    [UseEnumMemberConverter]
    [HttpGet("provider/{ukprn}/accountlegalentityId/{accountLegalEntityId}")]
    [ProducesResponseType(typeof(GetProviderEmployerRelationshipQueryResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(List<ValidationError>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetProviderEmployerRelationship(long? ukprn, long? accountLegalEntityId, CancellationToken cancellationToken)
    {
        GetProviderEmployerRelationshipQuery query = new(ukprn, accountLegalEntityId);

        ValidatedResponse<GetProviderEmployerRelationshipQueryResult> result = await _mediator.Send(query, cancellationToken);

        return GetResponse(result);
    }
}