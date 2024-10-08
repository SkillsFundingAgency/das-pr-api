﻿namespace SFA.DAS.PR.Domain.Entities;

public class AccountLegalEntity
{
    public long Id { get; set; }
    public string PublicHashedId { get; set; } = null!;
    public long AccountId { get; set; }
    public string Name { get; set; } = null!;
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
    public DateTime? Deleted { get; set; }

    public virtual Account Account { get; set; } = null!;
    public virtual List<AccountProviderLegalEntity> AccountProviderLegalEntities { get; set; } = [];
    public virtual List<Request> Requests { get; set; } = [];
}
