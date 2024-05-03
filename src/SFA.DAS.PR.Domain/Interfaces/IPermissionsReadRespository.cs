using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Domain.Interfaces;

public interface IPermissionsReadRespository
{
    Task<bool> GetPermissionsHas(long ukprn, string publicHashedId, List<Operation> operations, CancellationToken cancellationToken);
}