using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Data.Repositories;

public class PermissionsWriteRepository(IProviderRelationshipsDataContext providerRelationshipsDataContext) : IPermissionsWriteRepository
{
    public async Task CreatePermissions(Permission[] permissions, CancellationToken cancellationToken)
    {
        await providerRelationshipsDataContext.Permissions.AddRangeAsync(permissions, cancellationToken);
    }

    public void DeletePermissions(Permission[] permissions)
    {
        providerRelationshipsDataContext.Permissions.RemoveRange(permissions);
    }
}