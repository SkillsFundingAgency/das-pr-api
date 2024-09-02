using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Data.Repositories;

public class AccountReadRepository(IProviderRelationshipsDataContext _providerRelationshipsDataContext) : IAccountReadRepository
{
    public async Task<Account?> GetAccount(long Id, CancellationToken cancellationToken)
    {
        return await _providerRelationshipsDataContext.Accounts.FindAsync(Id, cancellationToken);
    }
}
