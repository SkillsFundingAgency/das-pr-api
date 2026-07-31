namespace SFA.DAS.PR.Domain.Interfaces;

public interface IProviderReadRepository
{
    Task<bool> ProviderExists(long ukprn, CancellationToken cancellationToken);
    Task<bool> ProviderRemoved(long ukprn, CancellationToken cancellationToken);
}
