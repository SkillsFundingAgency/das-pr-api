using Microsoft.EntityFrameworkCore;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Data.Repositories;

public class EmployerRelationshipsReadRepository(IProviderRelationshipsDataContext providerRelationshipsDataContext) : IEmployerRelationshipsReadRepository
{
    public async Task<Account?> GetRelationships(string accountHashedId, CancellationToken cancellationToken)
    {
        return await providerRelationshipsDataContext.Accounts
            .Include(acc => acc.AccountLegalEntities)
                .ThenInclude(ale => ale.AccountProviderLegalEntities)
                    .ThenInclude(aple => aple.Permissions)
            .Include(acc => acc.AccountLegalEntities)
                .ThenInclude(ale => ale.AccountProviderLegalEntities)
                    .ThenInclude(aple => aple.AccountProvider)
                        .ThenInclude(ap => ap.Account)
        .FirstOrDefaultAsync(a => a.HashedId == accountHashedId, cancellationToken);
    }
}
