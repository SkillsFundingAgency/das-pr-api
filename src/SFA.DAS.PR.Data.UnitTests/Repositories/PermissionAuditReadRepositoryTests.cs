using NUnit.Framework;
using SFA.DAS.PR.Data.Repositories;
using SFA.DAS.PR.Data.UnitTests.InMemoryDatabases;
using SFA.DAS.PR.Data.UnitTests.Setup;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Data.UnitTests.Repositories;

public class PermissionAuditReadRepositoryTests
{
    private readonly CancellationToken cancellationToken = CancellationToken.None;

    [Test]
    public async Task GetMostRecentPermissionAudit_Returns_Success()
    {
        PermissionsAudit permissionAudit = PermissionsAuditTestData.CreatePermissionsAudit(Guid.NewGuid());

        PermissionsAudit? result = null;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(GetMostRecentPermissionAudit_Returns_Success)}")
        )
        {
            await context.AddAsync(permissionAudit);
            await context.SaveChangesAsync(cancellationToken);

            PermissionAuditReadRepository sut = new(context);

            result = await sut.GetMostRecentPermissionAudit(permissionAudit.Ukprn, permissionAudit.AccountLegalEntityId, cancellationToken);
        }

        Assert.That(result, Is.Not.Null, $"result should not be null");
    }
}