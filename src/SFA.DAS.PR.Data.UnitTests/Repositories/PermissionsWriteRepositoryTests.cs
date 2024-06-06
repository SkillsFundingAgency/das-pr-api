using NUnit.Framework;
using SFA.DAS.PR.Data.Repositories;
using SFA.DAS.PR.Data.UnitTests.InMemoryDatabases;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Data.UnitTests.Repositories;

public class PermissionsWriteRepositoryTests
{
    private readonly CancellationToken cancellationToken = CancellationToken.None;

    [Test]
    [RecursiveMoqAutoData]
    public async Task CreatePermissions_Returns_Success(Permission[] permissions)
    {
        int permissionCount = 0;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(CreatePermissions_Returns_Success)}")
        )
        {
            PermissionsWriteRepository sut = new(context);

            await sut.CreatePermissions(permissions, cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            permissionCount = context.Permissions.Count();
        }

        Assert.That(permissionCount, Is.EqualTo(permissions.Count()), $"result should not be {permissions.Count()}");
    }
}
