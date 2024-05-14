using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Domain.Interfaces;

public interface IPermissionsReadRepository
{
    Task<bool> HasPermissionWithRelationship(long ukprn, Operation operation, CancellationToken cancellationToken);
    Task<List<Operation>> GetOperations(long ukprn, string publicHashedId, CancellationToken cancellationToken);
}
