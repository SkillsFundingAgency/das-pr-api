using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Data.UnitTests.Setup;
public class ProviderTestData
{
    public static Provider CreateProvider()
    {
        return new Provider()
        {
            Ukprn = 12345678,
            Name = "ProviderName",
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow
        };
    }
}
