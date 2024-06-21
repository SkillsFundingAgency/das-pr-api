using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Domain.Interfaces;

public interface IAccountProviderWriteRepository
{
    Task<AccountProvider> CreateAccountProvider(long ukprn, long accountId, CancellationToken cancellationToken);
}
