using AutoFixture;
using SFA.DAS.PR.Data.UnitTests.Helpers;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Data.UnitTests.Setup;

public static class PermissionsAuditTestData
{
    public static PermissionsAudit Create(Guid id) => TestHelpers.CreateFixture().Build<PermissionsAudit>()
        .With(a => a.Id, id)
        .With(a => a.Ukprn, 10000003)
        .With(a => a.AccountLegalEntityId, 3)
        .With(a => a.Eventtime, DateTime.Today)
        .With(a => a.Action, nameof(PermissionAction.PermissionCreated))
    .Create();
}
