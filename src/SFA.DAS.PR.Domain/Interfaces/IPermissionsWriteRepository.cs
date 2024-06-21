using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Domain.Interfaces;

public interface IPermissionsWriteRepository
{
    void CreatePermissions(IEnumerable<Permission> permissions);

    void DeletePermissions(IEnumerable<Permission> permissions);
}
