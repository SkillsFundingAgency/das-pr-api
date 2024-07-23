using AutoFixture;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.PR.Domain.Extensions;

namespace SFA.DAS.PR.Data.UnitTests.Setup;

public static class RequestTestData
{
    public static Request Create(Guid id, string requestStatus = "") {

        Operation[] operations = [Operation.CreateCohort, Operation.RecruitmentRequiresReview, Operation.Recruitment];

        List<PermissionRequest> permissionRequests = operations.Select(a => PermissionRequestsTestData.Create(a, id)).ToList();

        return FixtureBuilder.RecursiveMoqFixtureFactory().Build<Request>()
           .With(a => a.Id, id)
           .With(a => a.Ukprn, 10000003)
           .With(a => a.AccountLegalEntityId, 3)
           .With(a => a.RequestedDate, DateTime.Today)
           .With(a => a.Status, string.IsNullOrWhiteSpace(requestStatus) ? RequestStatus.New.ToLowerString() : requestStatus)
           .With(a => a.PermissionRequests, permissionRequests)
        .Create();
    }
}
