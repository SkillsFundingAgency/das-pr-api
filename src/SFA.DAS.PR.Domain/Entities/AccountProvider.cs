﻿namespace SFA.DAS.PR.Domain.Entities;

public class AccountProvider
{
    public long Id { get; set; }
    public long AccountId { get; set; }
    public long ProviderUkprn { get; set; }
    public DateTime Created { get; set; }

    public virtual Account Account { get; set; } = new();
    public virtual Provider Provider { get; set; } = new();
    public virtual List<AccountProviderLegalEntity> AccountProviderLegalEntities { get; set; } = new();
}
