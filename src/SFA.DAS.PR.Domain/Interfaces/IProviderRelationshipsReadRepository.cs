using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Models;

namespace SFA.DAS.PR.Domain.Interfaces;

public interface IProviderRelationshipsReadRepository
{
    Task<(List<ProviderRelationship>, int)> GetProviderRelationships(ProviderRelationshipsQueryFilters providerRelationshipsQueryOptions, CancellationToken cancellationToken);
}
