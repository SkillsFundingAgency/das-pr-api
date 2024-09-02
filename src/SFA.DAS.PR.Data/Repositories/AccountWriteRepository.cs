using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Data.Repositories;

public class AccountWriteRepository(IProviderRelationshipsDataContext _providerRelationshipsDataContext) : IAccountWriteRepository
{
    public async Task<Account> CreateAccount(Account account, CancellationToken cancellationToken)
    {
        await _providerRelationshipsDataContext.Accounts.AddAsync(account);
        return account;
    }
}
