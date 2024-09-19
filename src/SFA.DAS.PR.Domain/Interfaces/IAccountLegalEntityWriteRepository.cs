using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Domain.Interfaces;

public interface IAccountLegalEntityWriteRepository
{
    Task<AccountLegalEntity> CreateAccountLegalEntity(AccountLegalEntity accountLegalEntity, CancellationToken cancellationToken);
}
