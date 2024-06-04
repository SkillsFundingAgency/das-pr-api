using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Domain.Interfaces;

public interface IPermissionsAuditWriteRepository
{
    Task RecordPermissionsAudit(PermissionsAudit permissionsAudit, CancellationToken cancellationToken);
}