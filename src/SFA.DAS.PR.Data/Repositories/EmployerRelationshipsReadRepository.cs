using Microsoft.EntityFrameworkCore;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Data.Repositories;

public class EmployerRelationshipsReadRepository(IProviderRelationshipsDataContext providerRelationshipsDataContext) : IEmployerRelationshipsReadRepository
{
    public async Task<Account?> GetRelationships(long accountId, CancellationToken cancellationToken)
    {
        return await providerRelationshipsDataContext.Accounts
            .Include(acc => acc.AccountLegalEntities)
                .ThenInclude(ale => ale.AccountProviderLegalEntities)
                    .ThenInclude(aple => aple.Permissions)
            .Include(acc => acc.AccountLegalEntities.Where(x => x.Deleted == null))
                .ThenInclude(ale => ale.AccountProviderLegalEntities)
                    .ThenInclude(aple => aple.AccountProvider)
                        .ThenInclude(ap => ap.Provider)
            .Include(acc => acc.AccountLegalEntities)
                .ThenInclude(ale => ale.Requests)
                    .ThenInclude(r => r.PermissionRequests)
            .Include(acc => acc.AccountLegalEntities)
                .ThenInclude(ale => ale.Requests)
                    .ThenInclude(r => r.Provider)
        .FirstOrDefaultAsync(a => a.Id == accountId, cancellationToken);
    }
}
