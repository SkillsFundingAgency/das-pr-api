using AutoFixture;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Data.UnitTests.Setup;

public static class ProviderTestData
{
    public static Provider Create(long ukrn) => FixtureBuilder.RecursiveMoqFixtureFactory().Build<Provider>()
       .With(a => a.Ukprn, ukrn)
   .Create();
}
