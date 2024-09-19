using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Data.Repositories;

public class AccountLegalEntityWriteRepository(IProviderRelationshipsDataContext _providerRelationshipsDataContext) : IAccountLegalEntityWriteRepository
{
    public async Task<AccountLegalEntity> CreateAccountLegalEntity(AccountLegalEntity accountLegalEntity, CancellationToken cancellationToken)
    {
        await _providerRelationshipsDataContext.AccountLegalEntities.AddAsync(accountLegalEntity, cancellationToken);
        return accountLegalEntity;
    }
}
