using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.PR.Api.Authorization;
using SFA.DAS.PR.Api.Common;
using SFA.DAS.PR.Api.SwaggerExamples;
using SFA.DAS.PR.Application.AccountProviders.Queries.GetAccountProviders;
using SFA.DAS.PR.Application.Mediatr.Responses;
using Swashbuckle.AspNetCore.Filters;

namespace SFA.DAS.PR.Api.Controllers;

[ApiController]
[Route("accounts/{accountId:long}/providers")]
public class AccountProvidersController(IMediator _mediator) : ActionResponseControllerBase
{
    public override string ControllerName => "AccountProviders";

    [HttpGet]
    [Authorize(Policy = Policies.Integration)]
    [ProducesResponseType(typeof(GetAccountProvidersQueryResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(List<ValidationError>), StatusCodes.Status400BadRequest)]
    [SwaggerRequestExample(typeof(GetAccountProvidersQueryResult), typeof(GetAccountProvidersQueryResultExample))]
    public async Task<IActionResult> Get([FromRoute] long accountId, CancellationToken cancellationToken)
    {
        GetAccountProvidersQuery query = new(accountId);

        ValidatedResponse<GetAccountProvidersQueryResult> result = await _mediator.Send(query, cancellationToken);

        return GetResponse(result);
    }
}
