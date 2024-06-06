using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Data.Repositories;

public class AccountProviderLegalEntitiesWriteRepository(IProviderRelationshipsDataContext providerRelationshipsDataContext) : IAccountProviderLegalEntitiesWriteRepository
{
    public async Task<AccountProviderLegalEntity> CreateAccountProviderLegalEntity(long accountLegalEntityId, AccountProvider accountProvider, CancellationToken cancellationToken)
    {
        AccountProviderLegalEntity accountProviderLegalEntity = new()
        {
            AccountProvider = accountProvider,
            AccountLegalEntityId = accountLegalEntityId,
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow
        };

        await providerRelationshipsDataContext.AccountProviderLegalEntities.AddAsync(accountProviderLegalEntity, cancellationToken);

        return accountProviderLegalEntity;
    }
}