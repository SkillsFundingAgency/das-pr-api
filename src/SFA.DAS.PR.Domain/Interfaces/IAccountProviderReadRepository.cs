using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Domain.Interfaces;
public interface IAccountProviderReadRepository
{
    Task<AccountProvider?> GetAccountProvider(long? ukprn, long accountId, CancellationToken cancellationToken);
}
