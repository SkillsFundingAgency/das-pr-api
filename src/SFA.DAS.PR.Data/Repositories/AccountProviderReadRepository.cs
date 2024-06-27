using Microsoft.EntityFrameworkCore;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Data.Repositories;

public class AccountProviderReadRepository(IProviderRelationshipsDataContext _providerRelationshipsDataContext) : IAccountProviderReadRepository
{
    public Task<AccountProvider?> GetAccountProvider(long? ukprn, long accountId, CancellationToken cancellationToken)
    {
        return _providerRelationshipsDataContext.AccountProviders
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.AccountId == accountId && a.ProviderUkprn == ukprn, cancellationToken);
    }
}
