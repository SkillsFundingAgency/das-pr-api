namespace SFA.DAS.PR.Data.Repositories;

public interface IProviderReadRepository
{
    Task<bool> ProviderExists(long ukprn, CancellationToken cancellationToken);
}
