using Microsoft.EntityFrameworkCore;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Data.Repositories;

public class AccountProviderWriteRepository(IProviderRelationshipsDataContext _providerRelationshipsDataContext) : IAccountProviderWriteRepository
{
    public async Task<AccountProvider> CreateAccountProvider(long ukprn, long accountId, CancellationToken cancellationToken)
    {
        AccountProvider accountProvider = new()
        {
            ProviderUkprn = ukprn,
            AccountId = accountId,
            Created = DateTime.UtcNow
        };

        await _providerRelationshipsDataContext.AccountProviders.AddAsync(accountProvider, cancellationToken);

        return accountProvider;
    }

    public Task<AccountProvider?> GetAccountProvider(long? ukprn, long accountId, CancellationToken cancellationToken)
    {
        return _providerRelationshipsDataContext.AccountProviders
            .FirstOrDefaultAsync(a => a.AccountId == accountId && a.ProviderUkprn == ukprn, cancellationToken);
    }

}
