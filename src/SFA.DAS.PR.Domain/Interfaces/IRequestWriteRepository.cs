using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Domain.Interfaces;

public interface IRequestWriteRepository
{
    Task<Request?> GetRequest(Guid requestId, CancellationToken cancellationToken);
    Task<Request> CreateRequest(Request request, CancellationToken cancellationToken);
}
