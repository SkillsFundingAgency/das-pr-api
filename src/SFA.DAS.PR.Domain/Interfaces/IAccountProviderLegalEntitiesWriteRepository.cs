using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Domain.Interfaces;

public interface IAccountProviderLegalEntitiesWriteRepository
{
    Task CreateAccountProviderLegalEntity(AccountProviderLegalEntity accountProviderLegalEntity, CancellationToken cancellationToken);
}
