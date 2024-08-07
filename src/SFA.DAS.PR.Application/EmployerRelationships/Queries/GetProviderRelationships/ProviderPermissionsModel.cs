﻿using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.PR.Application.EmployerRelationships.Queries.GetProviderRelationships;

public class ProviderPermissionsModel
{
    public required long Ukprn { get; set; }

    public required string ProviderName { get; set; }

    public Operation[] Operations { get; set; } = [];

    public static implicit operator ProviderPermissionsModel(AccountProviderLegalEntity source) => new()
    {
        Ukprn = source.AccountProvider.ProviderUkprn,
        ProviderName = source.AccountProvider.Provider.Name,
        Operations = source.Permissions.Select(a => a.Operation).ToArray()
    };
}