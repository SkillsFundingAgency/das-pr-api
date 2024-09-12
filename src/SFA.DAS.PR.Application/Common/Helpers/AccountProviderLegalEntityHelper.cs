using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.PR.Application.Common.Helpers;

public static class AccountProviderLegalEntityHelper
{
    public static AccountProviderLegalEntity Create(AccountProvider accountProvider, long accountLegalEntityId, List<Operation> operations)
    {
        return new()
        {
            AccountProvider = accountProvider,
            AccountLegalEntityId = accountLegalEntityId,
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow,
            Permissions = operations.Select(a => new Permission()
            {
                Operation = a
            }).ToList()
        };
    }
}
