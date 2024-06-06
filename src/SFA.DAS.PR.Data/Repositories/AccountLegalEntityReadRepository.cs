using Microsoft.EntityFrameworkCore;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Data.Repositories;

public class AccountLegalEntityReadRepository(IProviderRelationshipsDataContext providerRelationshipsDataContext) : IAccountLegalEntityReadRepository
{
    public async Task<List<AccountLegalEntity>> GetAccountLegalEntities(long accountId, CancellationToken cancellationToken)
    {
        return await providerRelationshipsDataContext.AccountLegalEntities
            .Include(a => a.AccountProviderLegalEntities)
                .ThenInclude(a => a.Permissions)
            .Include(a => a.Account)
                .ThenInclude(a => a.AccountProviders)
                    .ThenInclude(a => a.Provider)
        .Where(ale => ale.AccountId == accountId)
        .ToListAsync(cancellationToken);
    }

    public async Task<AccountLegalEntity?> GetAccountLegalEntity(long accountLegalEntityId, CancellationToken cancellationToken)
    {
        return await providerRelationshipsDataContext.AccountLegalEntities.FirstOrDefaultAsync(a => a.Id == accountLegalEntityId, cancellationToken);
    }
}
