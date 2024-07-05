using AutoFixture;
using SFA.DAS.PR.Data.UnitTests.Helpers;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Data.UnitTests.Setup;

public static class PermissionRequestsTestData
{
    public static PermissionRequest Create(Operation operation, Guid requestId)
    {
        return TestHelpers.CreateFixture().Build<PermissionRequest>()
            .With(a => a.Operation, (int)operation)
            .With(a => a.RequestId, requestId)
        .Create();
    }
}
