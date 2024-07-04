using AutoFixture;
using SFA.DAS.PR.Data.UnitTests.Helpers;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Data.UnitTests.Setup;

public static class RequestTestData
{
    public static Request Create(Guid id) => TestHelpers.CreateFixture().Build<Request>()
       .With(a => a.Id, id)
        .With(a => a.Ukprn, 10000003)
        .With(a => a.AccountLegalEntityId, 3)
        .With(a => a.RequestedDate, DateTime.Today)
        .With(a => a.Status, "New")
   .Create();
}
