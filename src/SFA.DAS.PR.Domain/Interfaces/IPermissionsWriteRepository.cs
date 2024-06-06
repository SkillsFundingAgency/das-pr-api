using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Domain.Interfaces;

public interface IPermissionsWriteRepository
{
    Task CreatePermissions(Permission[] permissions, CancellationToken cancellationToken);

    void DeletePermissions(Permission[] permissions);
}
