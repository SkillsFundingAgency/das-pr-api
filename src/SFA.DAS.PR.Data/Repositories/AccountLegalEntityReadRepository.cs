using Microsoft.EntityFrameworkCore;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Data.Repositories;

public class AccountLegalEntityReadRepository(IProviderRelationshipsDataContext providerRelationshipsDataContext) : IAccountLegalEntityReadRepository
{
    public async Task<AccountLegalEntity?> GetAccountLegalEntity(long accountLegalEntityId, CancellationToken cancellationToken)
    {
        return await providerRelationshipsDataContext.AccountLegalEntities.FirstOrDefaultAsync(a => a.Id == accountLegalEntityId, cancellationToken);
    }
}
