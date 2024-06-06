using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Data.Repositories;

public class AccountProviderWriteRepository(IProviderRelationshipsDataContext providerRelationshipsDataContext) : IAccountProviderWriteRepository
{
    public async Task<AccountProvider> CreateAccountProvider(long ukprn, long accountId, CancellationToken cancellationToken)
    {
        AccountProvider accountProvider = new()
        {
            ProviderUkprn = ukprn,
            AccountId = accountId,
            Created = DateTime.UtcNow
        };

        await providerRelationshipsDataContext.AccountProviders.AddAsync(accountProvider, cancellationToken);

        return accountProvider;
    }
}