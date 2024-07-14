using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.PR.Api.Authorization;
using SFA.DAS.PR.Api.Common;
using SFA.DAS.PR.Application.ProviderRelationships.Queries.GetProviderRelationships;
using SFA.DAS.PR.Domain.QueryFilters;

namespace SFA.DAS.PR.Api.Controllers;

[Route("relationships/providers")]
[ApiController]
public class ProviderRelationshipsController(IMediator _mediator) : ActionResponseControllerBase
{
    public override string ControllerName => "ProviderRelationships";

    [HttpGet("{ukprn}")]
    [Authorize(Policy = Policies.Management)]
    [ProducesResponseType(typeof(GetProviderRelationshipsQueryResult), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(List<ValidationError>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetProviderRelationships([FromRoute] long ukprn, [FromQuery] ProviderRelationshipsRequestModel filters, CancellationToken cancellationToken)
    {
        GetProviderRelationshipsQuery query = GetQuery(ukprn, filters);
        query.Ukprn = ukprn;
        var result = await _mediator.Send(query, cancellationToken);
        return GetResponse(result);
    }

    private static GetProviderRelationshipsQuery GetQuery(long ukprn, ProviderRelationshipsRequestModel filters)
        => new()
        {
            Ukprn = ukprn,
            EmployerName = filters.EmployerName,
            HasCreateAdvertPermission = filters.HasCreateAdvertPermission,
            HasCreateAdvertWithReviewPermission = filters.HasCreateAdvertWithReviewPermission,
            HasCreateCohortPermission = filters.HasCreateCohortPermission,
            HasPendingRequest = filters.HasPendingRequest.GetValueOrDefault(),
            PageNumber = filters.PageNumber.GetValueOrDefault(),
            PageSize = filters.PageSize.GetValueOrDefault(),
        };
}
