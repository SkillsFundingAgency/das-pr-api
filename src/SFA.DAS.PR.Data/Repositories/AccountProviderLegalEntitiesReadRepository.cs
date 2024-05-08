using Microsoft.EntityFrameworkCore;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Data.Repositories;
public class AccountProviderLegalEntitiesReadRepository(IProviderRelationshipsDataContext _providerRelationshipsDataContext) : IAccountProviderLegalEntitiesReadRepository
{
    public Task<List<AccountProviderLegalEntity>> GetAccountProviderLegalEntities(long? ukprn, string? accountHashId, List<Operation> operations, CancellationToken cancellationToken)
    {
        return _providerRelationshipsDataContext.AccountProviderLegalEntities
            .Include(a => a.AccountProvider)
                .ThenInclude(a => a.Account)
            .Include(a => a.Permissions)
            .Include(a => a.AccountLegalEntity)
            .Where(t =>
                (!ukprn.HasValue || t.AccountProvider.ProviderUkprn == ukprn) &&
                (string.IsNullOrWhiteSpace(accountHashId) || t.AccountProvider.Account.HashedId == accountHashId) &&
                (t.Permissions.Any(p => operations.Contains(p.Operation))) &&
                t.AccountLegalEntity.Deleted == null
        ).ToListAsync(cancellationToken);
    }    
}