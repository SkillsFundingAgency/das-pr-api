using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.PR.Domain.QueryFilters;

namespace SFA.DAS.PR.Data.Repositories;

[ExcludeFromCodeCoverage]
public class ProviderRelationshipsReadRepository(IProviderRelationshipsDataContext _providerRelationshipsDataContext) : IProviderRelationshipsReadRepository
{
    public async Task<(List<ProviderRelationship>, int)> GetProviderRelationships(ProviderRelationshipsQueryOptions providerRelationshipsQueryOptions, CancellationToken cancellationToken)
    {
        var query = _providerRelationshipsDataContext.ProviderRelationships.Where(p => p.Ukprn == providerRelationshipsQueryOptions.Ukprn);

        if (!string.IsNullOrWhiteSpace(providerRelationshipsQueryOptions.EmployerName)) query = query.Where(p => p.EmployerName.Contains(providerRelationshipsQueryOptions.EmployerName));

        if (providerRelationshipsQueryOptions.HasPendingRequest) query = query.Where(p => p.RequestId != null);

        if (providerRelationshipsQueryOptions.HasCreateCohortPermission == true) query = query.Where(p => p.HasCreateCohortPermission == true);

        if (providerRelationshipsQueryOptions.HasCreateAdvertPermission == true && providerRelationshipsQueryOptions.HasCreateAdvertWithReviewPermission == true)
        {
            query = query.Where(p => p.HasCreateAdvertPermission == true || p.HasCreateAdvertWithReviewPermission == true);
        }
        else
        {
            if (providerRelationshipsQueryOptions.HasCreateAdvertPermission == true) query = query.Where(p => p.HasCreateAdvertPermission == true);
            if (providerRelationshipsQueryOptions.HasCreateAdvertWithReviewPermission == true) query = query.Where(p => p.HasCreateAdvertWithReviewPermission == true);
        }

        var count = await query.CountAsync(cancellationToken);

        query = query.OrderBy(p => p.EmployerName).Skip(providerRelationshipsQueryOptions.PageSize * (providerRelationshipsQueryOptions.PageNumber)).Take(providerRelationshipsQueryOptions.PageSize);

        var result = await query.ToListAsync(cancellationToken);

        return (result, count);
    }
}
