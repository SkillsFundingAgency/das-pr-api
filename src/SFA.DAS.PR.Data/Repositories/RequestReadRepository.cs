using Microsoft.EntityFrameworkCore;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Data.Repositories;

public class RequestReadRepository(IProviderRelationshipsDataContext providerRelationshipsDataContext) : IRequestReadRepository
{
    public async Task<Request?> GetRequest(long Ukprn, long AccountLegalEntityId, CancellationToken cancellationToken)
    {
        var requestQuery = providerRelationshipsDataContext.Requests.AsNoTracking().Where(a => a.AccountLegalEntityId == AccountLegalEntityId && a.Ukprn == Ukprn);

        return await requestQuery.OrderByDescending(a => a.RequestedDate).FirstOrDefaultAsync(cancellationToken);
    }
}