using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Data.UnitTests.Setup;

public static class AccountProviderTestData
{
    public static AccountProvider CreateAccountProvider(long id, long accountId, long ukprn)
    {
        return new()
        {
            Id = id,
            AccountId = accountId,
            ProviderUkprn = ukprn,
            Created = DateTime.Now.AddDays(-1)
        };
    }
}