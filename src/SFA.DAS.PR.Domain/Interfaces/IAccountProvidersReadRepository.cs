﻿using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Domain.Interfaces;

public interface IAccountProvidersReadRepository
{
    Task<List<AccountProvider>> GetAccountProviders(long accountId, CancellationToken cancellationToken);
}
