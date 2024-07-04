using AutoFixture;
using SFA.DAS.PR.Data.UnitTests.Helpers;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Data.UnitTests.Setup;

public static class AccountLegalEntityTestData
{
    public static AccountLegalEntity Create()
    {
        return TestHelpers.CreateFixture().Build<AccountLegalEntity>()
                      .Create();
    }
}
