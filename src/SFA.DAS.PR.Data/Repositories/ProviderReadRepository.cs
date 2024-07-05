using Microsoft.EntityFrameworkCore;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Data.Repositories;

public class ProviderReadRepository(IProviderRelationshipsDataContext _providerRelationshipsDataContext) : IProviderReadRepository
{
    public async Task<bool> ProviderExists(long ukprn, CancellationToken cancellationToken)
    {
        return await _providerRelationshipsDataContext.Providers
            .AsNoTracking()
            .AnyAsync(a => a.Ukprn == ukprn, cancellationToken);
    }
}
