using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Domain.Interfaces;

public interface IRequestReadRepository
{
    Task<Request?> GetRequest(long Ukprn, long AccountLegalEntityId, CancellationToken cancellationToken);
    Task<bool> RequestExists(long Ukprn, long AccountLegalEntityId, CancellationToken cancellationToken);
}
