﻿using SFA.DAS.PR.Application.EmployerRelationships.Queries.GetProviderRelationships;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Application.Permissions.Queries.GetEmployerRelationships;

public class AccountLegalEntityPermissionsModel
{
    public required long Id { get; set; }

    public required string PublicHashedId { get; set; }

    public required string Name { get; set; }

    public required long AccountId { get; set; }

    public List<ProviderPermissionsModel> Permissions { get; set; } = [];

    public static implicit operator AccountLegalEntityPermissionsModel(AccountLegalEntity source) => new()
    {
        Id = source.Id,
        PublicHashedId = source.PublicHashedId,
        Name = source.Name,
        AccountId = source.AccountId,
        Permissions = source.AccountProviderLegalEntities.Select(a => (ProviderPermissionsModel)a).ToList()
    };
}
