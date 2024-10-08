﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.PR.Api.Authorization;
using SFA.DAS.PR.Api.Common;
using SFA.DAS.PR.Api.Models;
using SFA.DAS.PR.Application.ProviderRelationships.Queries.GetProviderRelationships;

namespace SFA.DAS.PR.Api.Controllers;

[Route("")]
[ApiController]
public class ProviderRelationshipsController(IMediator _mediator) : ActionResponseControllerBase
{
    public override string ControllerName => "ProviderRelationships";

    [HttpGet("providers/{ukprn}/relationships")]
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
            SearchTerm = filters.SearchTerm,
            HasCreateCohortPermission = filters.HasCreateCohortPermission,
            HasRecruitmentPermission = filters.HasRecruitmentPermission,
            HasRecruitmentWithReviewPermission = filters.HasRecruitmentWithReviewPermission,
            HasNoRecruitmentPermissions = filters.HasNoRecruitmentPermission,
            HasPendingRequest = filters.HasPendingRequest,
            PageNumber = filters.PageNumber.GetValueOrDefault(),
            PageSize = filters.PageSize.GetValueOrDefault(),
        };
}
