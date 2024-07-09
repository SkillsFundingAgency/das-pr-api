using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.QueryFilters;

namespace SFA.DAS.PR.Domain.Interfaces;

public interface IProviderRelationshipsReadRepository
{
    Task<List<ProviderRelationship>> GetProviderRelationships(ProviderRelationshipsQueryOptions providerRelationshipsQueryOptions, CancellationToken cancellationToken);
}
