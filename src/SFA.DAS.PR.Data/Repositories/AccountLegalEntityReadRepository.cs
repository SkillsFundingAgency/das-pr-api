using Microsoft.EntityFrameworkCore;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Data.Repositories;

public class AccountLegalEntityReadRepository(IProviderRelationshipsDataContext providerRelationshipsDataContext) : IAccountLegalEntityReadRepository
{
    private readonly IProviderRelationshipsDataContext _providerRelationshipsDataContext = providerRelationshipsDataContext;

    public async Task<List<AccountLegalEntity>> GetAccountLegalEntiies(long accountId, CancellationToken cancellationToken)
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
}
