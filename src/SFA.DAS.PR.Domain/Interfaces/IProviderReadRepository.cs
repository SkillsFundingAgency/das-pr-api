namespace SFA.DAS.PR.Domain.Interfaces;

public interface IProviderReadRepository
{
    Task<bool> ProviderExists(long ukprn, CancellationToken cancellationToken);
}
