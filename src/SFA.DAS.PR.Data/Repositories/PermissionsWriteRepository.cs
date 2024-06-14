using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Data.Repositories;

public class PermissionsWriteRepository(IProviderRelationshipsDataContext providerRelationshipsDataContext) : IPermissionsWriteRepository
{
    public void CreatePermissions(IEnumerable<Permission> permissions)
    {
        providerRelationshipsDataContext.Permissions.AddRange(permissions);
    }

    public void DeletePermissions(IEnumerable<Permission> permissions)
    {
        providerRelationshipsDataContext.Permissions.RemoveRange(permissions);
    }
}