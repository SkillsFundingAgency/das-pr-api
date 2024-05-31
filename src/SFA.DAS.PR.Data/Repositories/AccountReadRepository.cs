using Microsoft.EntityFrameworkCore;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Data.Repositories;

public class AccountReadRepository(IProviderRelationshipsDataContext _providerRelationshipsDataContext) : IAccountReadRepository
{
    public async Task<bool> AccountExists(string accountHashedId, CancellationToken cancellationToken)
    {
        return await _providerRelationshipsDataContext.Accounts.AnyAsync(a => a.HashedId == accountHashedId);
    }
}
