using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Domain.Interfaces;

public interface IAccountLegalEntityReadRepository
{
    Task<List<AccountLegalEntity>> GetAccountLegalEntiies(long accountId, CancellationToken cancellationToken);
}

