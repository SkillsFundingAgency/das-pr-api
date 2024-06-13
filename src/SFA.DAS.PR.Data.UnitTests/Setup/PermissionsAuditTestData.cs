using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Data.UnitTests.Setup;

public static class PermissionsAuditTestData
{
    public static PermissionsAudit CreatePermissionsAudit(Guid id)
    {
        return new()
        {
            Id = id,
            Eventtime = DateTime.Today,
            Action = "PermissionCreated",
            Ukprn = 10000003,
            AccountLegalEntityId = 3,
            EmployerUserRef = Guid.NewGuid(),
            Operations = "[1,0]"
        };
    }
}