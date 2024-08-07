﻿using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.PR.Domain.Models;

namespace SFA.DAS.PR.Application.ProviderRelationships.Queries.GetProviderRelationships;

public class GetProviderRelationshipsQueryHandler(IProviderRelationshipsReadRepository _providerRelationshipsReadRepository) : IRequestHandler<GetProviderRelationshipsQuery, ValidatedResponse<GetProviderRelationshipsQueryResult>>
{
    const int defaultPageSize = 15;
    public async Task<ValidatedResponse<GetProviderRelationshipsQueryResult>> Handle(GetProviderRelationshipsQuery request, CancellationToken cancellationToken)
    {
        GetProviderRelationshipsQueryResult result = new();

        ProviderRelationshipsQueryFilters options = GetOptions(request);

        var (relationships, totalCount) = await _providerRelationshipsReadRepository.GetProviderRelationships(options, cancellationToken);

        result.HasAnyRelationships = await _providerRelationshipsReadRepository.HasAnyRelationship(request.Ukprn!.Value, cancellationToken);

        result.Employers = relationships.Select(r => (ProviderRelationshipModel)r);
        result.TotalCount = totalCount;
        result.PageNumber = options.PageNumber + 1;
        result.PageSize = options.PageSize;

        return new ValidatedResponse<GetProviderRelationshipsQueryResult>(result);
    }

    private static ProviderRelationshipsQueryFilters GetOptions(GetProviderRelationshipsQuery request)
        => new()
        {
            Ukprn = request.Ukprn,
            SearchTerm = request.SearchTerm,
            HasCreateCohortPermission = request.HasCreateCohortPermission,
            HasRecruitmentWithReviewPermission = request.HasRecruitmentWithReviewPermission,
            HasRecruitmentPermission = request.HasRecruitmentPermission,
            HasNoRecruitmentPermission = request.HasNoRecruitmentPermissions,
            HasPendingRequest = request.HasPendingRequest,
            PageSize = request.PageSize <= 0 ? defaultPageSize : request.PageSize,
            PageNumber = request.PageNumber <= 1 ? 0 : request.PageNumber - 1,
        };
}
