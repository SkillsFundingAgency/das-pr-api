using Microsoft.EntityFrameworkCore;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Data.Repositories;

public class AccountProvidersReadRepository(IProviderRelationshipsDataContext providerRelationshipsDataContext) : IAccountProvidersReadRepository
{
    private readonly IProviderRelationshipsDataContext _providerRelationshipsDataContext = providerRelationshipsDataContext;

    public async Task<List<AccountProvider>> GetAccountProviders(long accountId, CancellationToken cancellationToken)
    {
        return await _providerRelationshipsDataContext.AccountProviders
            .AsNoTracking()
            .Include(x => x.Provider)
            .Include(x => x.AccountProviderLegalEntities)
                .ThenInclude(y => y.AccountLegalEntity)
            .Include(x => x.AccountProviderLegalEntities)
                .ThenInclude(y => y.Permissions)
            .Where(x => x.AccountId == accountId)
            .ToListAsync(cancellationToken);
    }
}
