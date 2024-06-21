using Microsoft.EntityFrameworkCore;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Data.Repositories;

public class AccountLegalEntityReadRepository(IProviderRelationshipsDataContext _providerRelationshipsDataContext) : IAccountLegalEntityReadRepository
{
    public async Task<List<AccountLegalEntity>> GetAccountLegalEntities(long accountId, CancellationToken cancellationToken)
    {
        return await _providerRelationshipsDataContext.AccountLegalEntities
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
        return await _providerRelationshipsDataContext.AccountLegalEntities.FirstOrDefaultAsync(a => a.Id == accountLegalEntityId, cancellationToken);
    }

    public async Task<bool> AccountLegalEntityExists(long accountLegalEntityId, CancellationToken cancellationToken)
    {
        return await _providerRelationshipsDataContext.AccountLegalEntities.AnyAsync(a => a.Id == accountLegalEntityId, cancellationToken);
    }
}