using AutoFixture;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Data.UnitTests.Setup;

public static class AccountLegalEntityTestData
{
    public static AccountLegalEntity Create()
    {
        return FixtureBuilder.RecursiveMoqFixtureFactory()
            .Build<AccountLegalEntity>()
            .Create();
    }
}
