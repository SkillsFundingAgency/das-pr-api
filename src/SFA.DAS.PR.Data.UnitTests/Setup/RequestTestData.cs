using AutoFixture;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Data.UnitTests.Setup;

public static class RequestTestData
{
    public static Request Create(Guid id, RequestStatus requestStatus = RequestStatus.New, RequestType requestType = RequestType.CreateAccount) {

        Operation[] operations = [Operation.CreateCohort, Operation.RecruitmentRequiresReview, Operation.Recruitment];

        List<PermissionRequest> permissionRequests = operations.Select(a => PermissionRequestsTestData.Create(a, id)).ToList();

        return FixtureBuilder.RecursiveMoqFixtureFactory().Build<Request>()
           .With(a => a.Id, id)
           .With(a => a.Ukprn, 10000003)
           .With(a => a.AccountLegalEntityId, 3)
           .With(a => a.RequestedDate, DateTime.Today)
           .With(a => a.Status, requestStatus)
           .With(a => a.RequestType, requestType)
           .With(a => a.PermissionRequests, permissionRequests)
        .Create();
    }
}
