using NUnit.Framework;
using SFA.DAS.PR.Data.Repositories;
using SFA.DAS.PR.Data.UnitTests.InMemoryDatabases;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Data.UnitTests.Repositories;

public class PermissionsAuditWriteRepositoryTests
{
    private readonly CancellationToken cancellationToken = CancellationToken.None;

    [Test]
    [MoqAutoData]
    public async Task RecordPermissionsAudit_Returns_Success(
        PermissionsAudit permissionsAudit  
    )
    {
        PermissionsAudit? result = null;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(RecordPermissionsAudit_Returns_Success)}")
        )
        {
            PermissionsAuditWriteRepository sut = new(context);

            await sut.RecordPermissionsAudit(permissionsAudit, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            result = context.PermissionsAudit.FirstOrDefault(a => a.Ukprn == permissionsAudit.Ukprn);
        }

        Assert.That(result, Is.Not.Null, "result should not be null");
    }
}
