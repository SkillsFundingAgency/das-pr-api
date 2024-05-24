using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Domain.Interfaces;

public interface IEmployerRelationshipsReadRepository
{
    Task<Account?> GetRelationships(string accountHashedId, CancellationToken cancellationToken, long? ukprn = null, string? accountlegalentityPublicHashedId = "");
}
