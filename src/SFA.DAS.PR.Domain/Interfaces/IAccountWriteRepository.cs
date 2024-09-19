using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Domain.Interfaces;

public interface IAccountWriteRepository
{
    Task<Account> CreateAccount(Account account, CancellationToken cancellationToken);
}
