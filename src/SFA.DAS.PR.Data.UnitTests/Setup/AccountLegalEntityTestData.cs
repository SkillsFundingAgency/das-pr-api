using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Data.UnitTests.Setup;

public class AccountLegalEntityTestData
{
    public static AccountLegalEntity CreateAccountLegalEntity()
    {
        return new AccountLegalEntity()
        {
            Id = 1,
            PublicHashedId = "Hash",
            AccountId = 1,
            Name = "AccountLegalEntityName",
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow,
            Deleted = DateTime.UtcNow
        };
    }
}
