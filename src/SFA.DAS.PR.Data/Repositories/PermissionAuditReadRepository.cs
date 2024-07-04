using Microsoft.EntityFrameworkCore;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Data.Repositories;

public class PermissionAuditReadRepository(IProviderRelationshipsDataContext providerRelationshipsDataContext) : IPermissionAuditReadRepository
{
    public async Task<PermissionsAudit?> GetMostRecentPermissionAudit(long Ukprn, long AccountLegalEntityId, CancellationToken cancellationToken)
    {
        var query = providerRelationshipsDataContext.PermissionsAudit.AsNoTracking().Where(a => a.Ukprn == Ukprn && a.AccountLegalEntityId == AccountLegalEntityId);

        return await query.OrderByDescending(a => a.Eventtime).FirstOrDefaultAsync(cancellationToken);
    }
}