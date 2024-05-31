namespace SFA.DAS.PR.Domain.Interfaces;

public interface IAccountReadRepository
{
    Task<bool> AccountExists(string accountHashedId, CancellationToken cancellationToken);
}
