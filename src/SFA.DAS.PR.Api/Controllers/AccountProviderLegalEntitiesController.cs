using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.PR.Api.Authorization;
using SFA.DAS.PR.Api.Common;
using SFA.DAS.PR.Api.SwaggerExamples;
using SFA.DAS.PR.Application.AccountProviderLegalEntities.Queries.GetAccountProviderLegalEntities;
using SFA.DAS.PR.Application.Mediatr.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace SFA.DAS.PR.Api.Controllers;

[ApiController]
[Route("accountproviderlegalentities")]
public class AccountProviderLegalEntitiesController(IMediator _mediator) : ActionResponseControllerBase
{
    public override string ControllerName => "AccountProviderLegalEntities";

    [HttpGet]
    [Authorize(Policy = Policies.Integration)]
    [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [SwaggerRequestExample(typeof(GetAccountProviderLegalEntitiesQueryResult), typeof(GetAccountProviderLegalEntitiesQueryResultExample))]
    public async Task<IActionResult> Get([FromQuery] GetAccountProviderLegalEntitiesQuery query, CancellationToken cancellationToken)
    {
        ValidatedResponse<GetAccountProviderLegalEntitiesQueryResult> result = await _mediator.Send(query, cancellationToken);

        return GetResponse(result);
    }
}
