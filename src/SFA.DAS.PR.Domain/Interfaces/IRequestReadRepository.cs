using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Domain.Interfaces;

public interface IRequestReadRepository
{
    Task<Request?> GetRequest(Guid requestId, CancellationToken cancellationToken);
    Task<Request?> GetRequest(long Ukprn, long AccountLegalEntityId, CancellationToken cancellationToken);
    Task<Request?> GetRequest(long Ukprn, string payee, RequestStatus[] requestStatuses, CancellationToken cancellationToken);
    Task<bool> RequestExists(long Ukprn, long AccountLegalEntityId, RequestStatus[] requestStatuses, CancellationToken cancellationToken);
    Task<bool> RequestExists(long Ukprn, string EmployerPAYE, RequestStatus[] requestStatuses, CancellationToken cancellationToken);
    Task<bool> RequestExists(Guid RequestId, RequestStatus[] requestStatuses, RequestType? requestType, CancellationToken cancellationToken);
}
