using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.PR.Domain.Interfaces;
public interface IAccountProviderLegalEntitiesReadRepository
{
    Task<List<AccountProviderLegalEntity>> GetAccountProviderLegalEntities(long? ukprn, string? accountHashId, List<Operation> operations, CancellationToken cancellationToken);
    Task<AccountProviderLegalEntity?> GetAccountProviderLegalEntity(long? ukprn, long accountLegalEntityId, CancellationToken cancellationToken);
}
