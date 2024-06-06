﻿using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Domain.Interfaces;

public interface IAccountLegalEntityReadRepository
{
    Task<AccountLegalEntity?> GetAccountLegalEntity(long accountLegalEntityId, CancellationToken cancellationToken);
    Task<List<AccountLegalEntity>> GetAccountLegalEntities(long accountId, CancellationToken cancellationToken);
}