﻿using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.PR.Data.UnitTests.Setup;

public static class AccountTestData
{
    public static Account CreateAccount(long id)
    {
        return new()
        {
            Id = id,
            HashedId = "hash",
            PublicHashedId = "phash",
            Name = "Name",
            Created = DateTime.Now.AddDays(-1),
            Updated = DateTime.Now
        };
    }

    public static List<Account> CreateAccounts()
    {
        return new()
        {
            new()
            {
                Id = 1001,
                HashedId = Guid.NewGuid().ToString(),
                PublicHashedId = Guid.NewGuid().ToString(),
                Name = "AccountName",
                Created = DateTime.UtcNow.AddDays(-1),
                Updated = DateTime.UtcNow,
                AccountProviders = new()
                {
                    new()
                    {
                        Id = 1002,
                        AccountId = 1001,
                        ProviderUkprn = 1006,
                        Created = DateTime.UtcNow.AddDays(-1),
                        Provider = new()
                        {
                            Ukprn = 1006,
                            Name = "ProviderName",
                            Created = DateTime.UtcNow.AddDays(-1),
                            Updated = DateTime.UtcNow
                        },
                        AccountProviderLegalEntities = new()
                        {
                            new()
                            {
                                Id = 1005,
                                AccountProviderId = 1002,
                                AccountLegalEntityId = 1004,
                                Created = DateTime.UtcNow.AddDays(-1),
                                Updated = DateTime.UtcNow,
                                Permissions = new()
                                {
                                    new()
                                    {
                                        AccountProviderLegalEntityId = 1005,
                                        Id = 106,
                                        Operation = Operation.CreateCohort
                                    }
                                },
                            }
                        }
                    }
                },
                AccountLegalEntities = new()
                {
                    new()
                    {
                        Id = 1004,
                        PublicHashedId = Guid.NewGuid().ToString(),
                        AccountId = 1001,
                        Name = "AccountLegalEntityName",
                        Created = DateTime.UtcNow.AddDays(-1),
                        Updated = DateTime.UtcNow,
                        Deleted = null
                    }
                }
            }
        };
    }
}
