﻿using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.PR.Data.UnitTests.Setup;

public static class AccountProviderLegalEntityTestData
{
    public static AccountProviderLegalEntity CreateAccountProviderLegalEntity(Account account)
    {
        return new()
        {
            Id = 1001,
            AccountProviderId = 1002,
            AccountLegalEntityId = 1003,
            Created = DateTime.UtcNow.AddDays(-1),
            Updated = DateTime.UtcNow,
            Permissions = new List<Permission>()
            {
                new ()
                {
                    Id = 1,
                    AccountProviderLegalEntityId = 1001,
                    Operation = Operation.Recruitment
                }
            },
            AccountLegalEntity = new()
            {
                Id = 1003,
                PublicHashedId = "hash",
                AccountId = account.Id,
                Account = account,
                Name = "Name",
                Created = DateTime.Now.AddDays(-1),
                Updated = DateTime.Now,
                Deleted = null
            },
            AccountProvider = new()
            {
                Id = 1002,
                AccountId = account.Id,
                Account = account,
                ProviderUkprn = 1005,
                Created = DateTime.Now.AddDays(-1),
                Provider = new()
                {
                    Ukprn = 1005,
                    Name = "Name",
                    Created = DateTime.Now.AddDays(-1),
                    Updated = DateTime.Now
                }
            }
        };
    }

    public static List<AccountProviderLegalEntity> CreateAccountProviderLegalEntitiesWithPermissions(Account account)
    {
        var requestId = Guid.NewGuid();

        return new()
        {
            new()
            {
                Id = 1001,
                AccountProviderId = 1002,
                AccountLegalEntityId = 1003,
                Created = DateTime.UtcNow.AddDays(-1),
                Updated = DateTime.UtcNow,
                Permissions = new List<Permission>()
                {
                    new ()
                    {
                        Id = 1,
                        AccountProviderLegalEntityId = 1001,
                        Operation = Operation.Recruitment
                    }
                },
                AccountLegalEntity = new()
                {
                    Id = 1003,
                    PublicHashedId = "hash",
                    AccountId = account.Id,
                    Account = account,
                    Name = "Name",
                    Created = DateTime.Now.AddDays(-1),
                    Updated = DateTime.Now,
                    Deleted = null,
                    Requests = new ()
                    {
                        new Request()
                        {
                            Id = requestId,
                            RequestType = RequestType.CreateAccount,
                            AccountLegalEntityId = 1003,
                            Ukprn = 1005,
                            RequestedBy = Guid.NewGuid().ToString(),
                            PermissionRequests = new()
                            {
                                new PermissionRequest()
                                {
                                    Id = Guid.NewGuid(),
                                    Operation = (short)Operation.Recruitment,
                                    RequestId = requestId
                                }
                            }
                        }
                    }
                },
                AccountProvider = new()
                {
                    Id = 1002,
                    AccountId = account.Id,
                    Account = account,
                    ProviderUkprn = 1005,
                    Created = DateTime.Now.AddDays(-1),
                    Provider = new()
                    {
                        Ukprn = 1005,
                        Name = "Name",
                        Created = DateTime.Now.AddDays(-1),
                        Updated = DateTime.Now
                    }
                }
            }
        };
    }
}
