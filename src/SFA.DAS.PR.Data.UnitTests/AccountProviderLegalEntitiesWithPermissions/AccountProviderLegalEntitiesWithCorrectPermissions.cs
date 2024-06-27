using AutoFixture;
using NUnit.Framework;
using SFA.DAS.PR.Data.Repositories;
using SFA.DAS.PR.Data.UnitTests.InMemoryDatabases;
using SFA.DAS.PR.Data.UnitTests.Setup;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.PR.Data.UnitTests.AccountProviderLegalEntitiesWithPermissions;
public class AccountProviderLegalEntitiesWithCorrectPermissions
{
    private CancellationToken cancellationToken = CancellationToken.None;

    private Fixture _fixture = null!;

    [SetUp]
    public void Setup()
    {
        _fixture = new Fixture();
        _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
        _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
    }

    [Test]
    public async Task AccountProviderLegalEntities_Has_Permission()
    {
        Account account = AccountTestData.CreateAccount(1004);

        List<AccountProviderLegalEntity> accountProviderLegalEntitiesToAdd =
            AccountProviderLegalEntityTestData.CreateAccountProviderLegalEntitiesWithPermissions(account);

        long ukprn = accountProviderLegalEntitiesToAdd[0].AccountProvider.ProviderUkprn;

        bool hasPermission;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(AccountProviderLegalEntities_Has_Permission)}")
        )
        {
            await context.AddAsync(account);
            await context.AddRangeAsync(accountProviderLegalEntitiesToAdd);
            await context.SaveChangesAsync(cancellationToken);

            PermissionsReadRepository sut = new(context);

            hasPermission = await sut.HasPermissionWithRelationship(ukprn, Operation.Recruitment, cancellationToken);
        }

        Assert.That(hasPermission, Is.True);
    }

    [Test]
    public async Task AccountProviderLegalEntities_Does_Not_Have_Permission()
    {
        Account account = AccountTestData.CreateAccount(1004);

        List<AccountProviderLegalEntity> accountProviderLegalEntitiesToAdd =
            AccountProviderLegalEntityTestData.CreateAccountProviderLegalEntitiesWithPermissions(account);

        long ukprn = accountProviderLegalEntitiesToAdd[0].AccountProvider.ProviderUkprn;

        bool hasPermission;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(AccountProviderLegalEntities_Does_Not_Have_Permission)}")
        )
        {
            await context.AddAsync(account);
            await context.AddRangeAsync(accountProviderLegalEntitiesToAdd);
            await context.SaveChangesAsync(cancellationToken);

            PermissionsReadRepository sut = new(context);

            hasPermission = await sut.HasPermissionWithRelationship(ukprn, Operation.CreateCohort, cancellationToken);
        }

        Assert.That(hasPermission, Is.False);
    }
}
