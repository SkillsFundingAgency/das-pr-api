using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Domain.Interfaces;

public interface IEmployerRelationshipsReadRepository
{
    Task<Account?> GetRelationships(long accountId, CancellationToken cancellationToken);

    Task<bool> AccountIdExists(long accountId, CancellationToken cancellationToken);
}
