using Microsoft.EntityFrameworkCore;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Data.Repositories;

public class EmployerRelationshipsReadRepository(IProviderRelationshipsDataContext providerRelationshipsDataContext) : IEmployerRelationshipsReadRepository
{
    public async Task<Account?> GetRelationships(string accountHashedId, CancellationToken cancellationToken, long? ukprn = null, string? accountlegalentityPublicHashedId = null)
    {
        return await providerRelationshipsDataContext.Accounts
            .Include(acc => acc.AccountLegalEntities.Where(a => string.IsNullOrWhiteSpace(accountlegalentityPublicHashedId) || a.PublicHashedId == accountlegalentityPublicHashedId))
                .ThenInclude(ale => ale.AccountProviderLegalEntities.Where(a => !ukprn.HasValue || a.AccountProvider.ProviderUkprn == ukprn.Value))
                    .ThenInclude(aple => aple.Permissions)
            .Include(acc => acc.AccountLegalEntities.Where(a => string.IsNullOrWhiteSpace(accountlegalentityPublicHashedId) || a.PublicHashedId == accountlegalentityPublicHashedId))
                .ThenInclude(ale => ale.AccountProviderLegalEntities.Where(a => !ukprn.HasValue || a.AccountProvider.ProviderUkprn == ukprn.Value))
                    .ThenInclude(aple => aple.AccountProvider)
                        .ThenInclude(ap => ap.Provider)
        .FirstOrDefaultAsync(a => a.HashedId == accountHashedId, cancellationToken);
    }
}