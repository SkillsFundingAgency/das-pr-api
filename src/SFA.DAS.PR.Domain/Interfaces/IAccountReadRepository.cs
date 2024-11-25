using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Domain.Interfaces;

public interface IAccountReadRepository
{
    Task<Account?> GetAccount(long id, CancellationToken cancellationToken);

    Task<bool> AccountIdExists(long accountId, CancellationToken cancellationToken);
}
