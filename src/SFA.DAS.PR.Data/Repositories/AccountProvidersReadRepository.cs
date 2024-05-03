using Microsoft.EntityFrameworkCore;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Data.Repositories;

public class AccountProvidersReadRepository(IProviderRelationshipsDataContext providerRelationshipsDataContext) : IAccountProvidersReadRepository
{
    private readonly IProviderRelationshipsDataContext _providerRelationshipsDataContext = providerRelationshipsDataContext;

    public async Task<List<AccountProvider>> GetAccountProviders(string accountHashedId, CancellationToken cancellationToken)
    {
        Account? account = await _providerRelationshipsDataContext.Accounts.FirstOrDefaultAsync(x => x.PublicHashedId == accountHashedId, cancellationToken);

        if (account == null)
            return new List<AccountProvider>();

        var accountProviders = await _providerRelationshipsDataContext.AccountProviders
            .AsNoTracking()
            .Include(x => x.Provider)
            .Include(x => x.AccountProviderLegalEntities)
                .ThenInclude(y => y.AccountLegalEntity)
            .Include(x => x.AccountProviderLegalEntities)
                .ThenInclude(y => y.Permissions)
            .Where(x => x.AccountId == account.Id)
            .ToListAsync(cancellationToken);

        return accountProviders;
    }
}