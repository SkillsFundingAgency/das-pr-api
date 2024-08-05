using Microsoft.EntityFrameworkCore;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Data.Repositories;

public class RequestReadRepository(IProviderRelationshipsDataContext providerRelationshipsDataContext) : IRequestReadRepository
{
    public async Task<Request?> GetRequest(Guid requestId, CancellationToken cancellationToken)
    {
        return await providerRelationshipsDataContext.Requests
            .Include(a => a.PermissionRequests)
            .Include(a => a.Provider)
        .AsNoTracking()
        .FirstOrDefaultAsync(a => a.Id == requestId, cancellationToken);
    }

    public async Task<Request?> GetRequest(long Ukprn, long AccountLegalEntityId, CancellationToken cancellationToken)
    {
        var requestQuery = providerRelationshipsDataContext.Requests
            .Include(a => a.PermissionRequests)
        .AsNoTracking()
        .Where(a => a.AccountLegalEntityId == AccountLegalEntityId && a.Ukprn == Ukprn);

        return await requestQuery.OrderByDescending(a => a.RequestedDate).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Request?> GetRequest(long Ukprn, string payee, RequestStatus[] requestStatuses, CancellationToken cancellationToken)
    {
        return await providerRelationshipsDataContext.Requests
            .Include(a => a.PermissionRequests)
            .Include(a => a.Provider)
        .AsNoTracking()
        .FirstOrDefaultAsync(a => 
            a.Ukprn == Ukprn && 
            a.EmployerPAYE == payee &&
            (requestStatuses.Count() == 0 || requestStatuses.Contains(a.Status)),
            cancellationToken
        );
    }

    public async Task<bool> RequestExists(long Ukprn, long AccountLegalEntityId, RequestStatus[] requestStatuses, CancellationToken cancellationToken)
    {
        return await providerRelationshipsDataContext.Requests
            .AsNoTracking()
            .AnyAsync(a => 
                a.AccountLegalEntityId == AccountLegalEntityId && 
                a.Ukprn == Ukprn &&
                (requestStatuses.Count() == 0 || requestStatuses.Contains(a.Status)),
                cancellationToken
        );
    }
}