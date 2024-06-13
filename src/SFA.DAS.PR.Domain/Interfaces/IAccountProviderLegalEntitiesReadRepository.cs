using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Domain.Interfaces;
public interface IAccountProviderLegalEntitiesReadRepository
{
    Task<List<AccountProviderLegalEntity>> GetAccountProviderLegalEntities(long? ukprn, string? accountHashId, List<Operation> operations, CancellationToken cancellationToken);
    Task<AccountProviderLegalEntity?> GetAccountProviderLegalEntity(long? ukprn, long accountLegalEntityId, CancellationToken cancellationToken);
    Task<AccountProviderLegalEntity?> GetAccountProviderLegalEntityByProvider(long ukprn, long accountLegalEntityId, CancellationToken cancellationToken);
}
