
using Microsoft.EntityFrameworkCore;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Data.Repositories;

public class RequestWriteRepository(IProviderRelationshipsDataContext _providerRelationshipsDataContext) : IRequestWriteRepository
{
    public async Task<Request?> GetRequest(Guid requestId, CancellationToken cancellationToken)
    {
        return await _providerRelationshipsDataContext.Requests
            .Include(a => a.PermissionRequests)
        .FirstOrDefaultAsync(a => a.Id == requestId, cancellationToken);
    }

    public async Task<Request> CreateRequest(Request request, CancellationToken cancellationToken)
    {
        await _providerRelationshipsDataContext.Requests.AddAsync(request, cancellationToken);
        return request;
    }
}
