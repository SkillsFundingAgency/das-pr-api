using Microsoft.EntityFrameworkCore;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Data.Repositories;

public class AccountProvidersReadRepository(IProviderRelationshipsDataContext providerRelationshipsDataContext) : IAccountProvidersReadRepository
{
    private readonly IProviderRelationshipsDataContext _providerRelationshipsDataContext = providerRelationshipsDataContext;

    public async Task<List<AccountLegalEntity>> GetAccountProviders(long accountId, CancellationToken cancellationToken)
    {
        List<AccountLegalEntity> result = await _providerRelationshipsDataContext.AccountLegalEntities
            .Include(a => a.AccountProviderLegalEntities)
                .ThenInclude(a => a.Permissions)
            .Include(a => a.Account)
                .ThenInclude(a => a.AccountProviders)
                    .ThenInclude(a => a.Provider)
        .Where(ale => ale.AccountId == accountId)
        .ToListAsync(cancellationToken);

        return result;

        //return await _providerRelationshipsDataContext.AccountProviders
        //    .AsNoTracking()
        //    .Include(x => x.Provider)
        //    .Include(x => x.AccountProviderLegalEntities)
        //        .ThenInclude(y => y.AccountLegalEntity)
        //    .Include(x => x.AccountProviderLegalEntities)
        //        .ThenInclude(y => y.Permissions)
        //    .Where(x => x.AccountId == accountId)
        //    .ToListAsync(cancellationToken);
    }
}
