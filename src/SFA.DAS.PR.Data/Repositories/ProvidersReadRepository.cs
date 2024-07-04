using Microsoft.EntityFrameworkCore;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Data.Repositories;

public class ProvidersReadRepository(IProviderRelationshipsDataContext _providerRelationshipsDataContext) : IProvidersReadRepository
{
    public async Task<bool> ProviderExists(long? ukprn, CancellationToken cancellationToken)
    {
        return await _providerRelationshipsDataContext.Providers
            .AsNoTracking()
            .AnyAsync(a => a.Ukprn == ukprn, cancellationToken);
    }
}