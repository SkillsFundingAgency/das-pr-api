using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Domain.Interfaces;

public interface IPermissionAuditReadRepository
{
    Task<PermissionsAudit?> GetMostRecentPermissionAudit(long Ukprn, long AccountLegalEntityId, CancellationToken cancellationToken);
}