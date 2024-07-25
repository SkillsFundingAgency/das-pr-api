using Microsoft.EntityFrameworkCore;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Data.Repositories;

public class RequestReadRepository(IProviderRelationshipsDataContext providerRelationshipsDataContext) : IRequestReadRepository
{
    public async Task<Request?> GetRequest(long Ukprn, long AccountLegalEntityId, CancellationToken cancellationToken)
    {
        var requestQuery = providerRelationshipsDataContext.Requests
            .Include(a => a.PermissionRequests)
        .AsNoTracking()
        .Where(a => a.AccountLegalEntityId == AccountLegalEntityId && a.Ukprn == Ukprn);

        return await requestQuery.OrderByDescending(a => a.RequestedDate).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> RequestExists(long Ukprn, long AccountLegalEntityId, CancellationToken cancellationToken)
    {
        string statusSent = RequestStatus.Sent.ToString();
        string statusNew = RequestStatus.New.ToString();

        return await providerRelationshipsDataContext.Requests
            .AsNoTracking()
            .AnyAsync(a => 
                a.AccountLegalEntityId == AccountLegalEntityId && 
                a.Ukprn == Ukprn && 
                (a.Status == statusNew || a.Status == statusSent),
                cancellationToken
        );
    }
}