using Microsoft.EntityFrameworkCore;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Data.Repositories;

public class EmployerRelationshipsReadRepository(IProviderRelationshipsDataContext providerRelationshipsDataContext) : IEmployerRelationshipsReadRepository
{
    public async Task<Account?> GetRelationships(string accountHashedId, long? ukprn, string? accountlegalentityPublicHashedId, CancellationToken cancellationToken)
    {
        var query = providerRelationshipsDataContext.Accounts.AsQueryable();

        if(ukprn != null && !string.IsNullOrWhiteSpace(accountlegalentityPublicHashedId))
        {
            query = query.Include(acc => acc.AccountLegalEntities.Where(a => a.PublicHashedId == accountlegalentityPublicHashedId))
                .ThenInclude(ale => ale.AccountProviderLegalEntities.Where(a => a.AccountProvider.ProviderUkprn == ukprn.Value))
                    .ThenInclude(aple => aple.Permissions)
            .Include(acc => acc.AccountLegalEntities.Where(a =>a.PublicHashedId == accountlegalentityPublicHashedId))
                .ThenInclude(ale => ale.AccountProviderLegalEntities.Where(a => a.AccountProvider.ProviderUkprn == ukprn.Value))
                    .ThenInclude(aple => aple.AccountProvider)
                        .ThenInclude(ap => ap.Provider);
        }
        else
        {
            query = query.Include(acc => acc.AccountLegalEntities)
                .ThenInclude(ale => ale.AccountProviderLegalEntities)
                    .ThenInclude(aple => aple.Permissions)
            .Include(acc => acc.AccountLegalEntities)
                .ThenInclude(ale => ale.AccountProviderLegalEntities)
                    .ThenInclude(aple => aple.AccountProvider)
                        .ThenInclude(ap => ap.Provider);
        }

        return await query
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.HashedId == accountHashedId, cancellationToken);
    }
}