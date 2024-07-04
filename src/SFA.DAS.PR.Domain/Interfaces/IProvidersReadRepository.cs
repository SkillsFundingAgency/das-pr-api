namespace SFA.DAS.PR.Domain.Interfaces;

public interface IProvidersReadRepository
{
    Task<bool> ProviderExists(long? ukprn, CancellationToken cancellationToken);
}