using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Domain.Interfaces;

public interface IAccountProviderLegalEntitiesWriteRepository
{
    Task<AccountProviderLegalEntity> CreateAccountProviderLegalEntity(long accountLegalEntityId, AccountProvider accountProvider, CancellationToken cancellationToken);
}
