using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Data.Repositories;

public class PermissionsAuditWriteRepository(IProviderRelationshipsDataContext providerRelationshipsDataContext) : IPermissionsAuditWriteRepository
{
    public async Task RecordPermissionsAudit(PermissionsAudit permissionsAudit, CancellationToken cancellationToken)
    {
        await providerRelationshipsDataContext.PermissionsAudit.AddAsync(permissionsAudit, cancellationToken);
    }
}