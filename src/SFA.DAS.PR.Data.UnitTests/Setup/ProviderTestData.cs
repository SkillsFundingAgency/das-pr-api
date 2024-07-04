using AutoFixture;
using SFA.DAS.PR.Data.UnitTests.Helpers;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Data.UnitTests.Setup;

public static class ProviderTestData
{
    public static Provider Create(long ukrn) => TestHelpers.CreateFixture().Build<Provider>()
       .With(a => a.Ukprn, ukrn)
   .Create();
}
