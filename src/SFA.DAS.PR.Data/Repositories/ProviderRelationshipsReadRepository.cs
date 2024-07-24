using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.PR.Data.Extensions;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.PR.Domain.Models;

namespace SFA.DAS.PR.Data.Repositories;

[ExcludeFromCodeCoverage]
public class ProviderRelationshipsReadRepository(IProviderRelationshipsDataContext _providerRelationshipsDataContext) : IProviderRelationshipsReadRepository
{
    public async Task<(List<ProviderRelationship>, int)> GetProviderRelationships(ProviderRelationshipsQueryFilters providerRelationshipsQueryOptions, CancellationToken cancellationToken)
    {
        var query = _providerRelationshipsDataContext.ProviderRelationships.Where(BuildPredicate(providerRelationshipsQueryOptions));

        var count = await query.CountAsync(cancellationToken);

        query = query.OrderBy(p => p.EmployerName).Skip(providerRelationshipsQueryOptions.PageSize * (providerRelationshipsQueryOptions.PageNumber)).Take(providerRelationshipsQueryOptions.PageSize);

        var result = await query.ToListAsync(cancellationToken);

        return (result, count);
    }

    private static Expression<Func<ProviderRelationship, bool>> BuildPredicate(ProviderRelationshipsQueryFilters filters)
    {
        Expression<Func<ProviderRelationship, bool>> predicate = p => p.Ukprn == filters.Ukprn;

        if (!string.IsNullOrWhiteSpace(filters.SearchTerm)) predicate = predicate.And(p => p.EmployerName.Contains(filters.SearchTerm) || p.AgreementId == filters.SearchTerm);

        if (filters.HasPendingRequest.HasValue && filters.HasPendingRequest == true) predicate = predicate.And(p => p.RequestId != null);
        if (filters.HasPendingRequest.HasValue && filters.HasPendingRequest == false) predicate = predicate.And(p => p.RequestId == null);

        if (filters.HasCreateCohortPermission.HasValue) predicate = predicate.And(p => p.HasCreateCohortPermission == filters.HasCreateCohortPermission);

        if (filters.HasRecruitmentPermission == true && filters.HasRecruitmentWithReviewPermission == true)
        {
            predicate = predicate.And(p => p.HasRecruitmentPermission || p.HasRecruitmentWithReviewPermission);
        }
        else
        {
            if (filters.HasRecruitmentPermission.HasValue)
            {
                predicate = predicate.And(p => p.HasRecruitmentPermission == filters.HasRecruitmentPermission);
            }

            if (filters.HasRecruitmentWithReviewPermission.HasValue)
            {
                predicate = predicate.And(p => p.HasRecruitmentWithReviewPermission == filters.HasRecruitmentWithReviewPermission);
            }
        }
        return predicate;
    }
}
