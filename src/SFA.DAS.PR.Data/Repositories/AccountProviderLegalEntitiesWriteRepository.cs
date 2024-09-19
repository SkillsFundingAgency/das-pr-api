using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Data.Repositories;

public class AccountProviderLegalEntitiesWriteRepository(IProviderRelationshipsDataContext providerRelationshipsDataContext) : IAccountProviderLegalEntitiesWriteRepository
{
    public async Task CreateAccountProviderLegalEntity(AccountProviderLegalEntity accountProviderLegalEntity, CancellationToken cancellationToken)
    {
        await providerRelationshipsDataContext.AccountProviderLegalEntities.AddAsync(accountProviderLegalEntity, cancellationToken);
    }
}