using AutoFixture;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.ProviderRelationships.Types.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Data.UnitTests.Setup;

public static class PermissionRequestsTestData
{
    public static PermissionRequest Create(Operation operation, Guid requestId)
    {
        return FixtureBuilder.RecursiveMoqFixtureFactory().Build<PermissionRequest>()
            .With(a => a.Operation, (int)operation)
            .With(a => a.RequestId, requestId)
        .Create();
    }
}
