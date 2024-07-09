using NUnit.Framework;
using SFA.DAS.PR.Data.Repositories;
using SFA.DAS.PR.Data.UnitTests.InMemoryDatabases;
using SFA.DAS.PR.Data.UnitTests.Setup;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.PR.Data.UnitTests.Repositories;

public class PermissionsReadRepositoryTests
{
    private readonly CancellationToken cancellationToken = CancellationToken.None;

    [Test]
    [MoqAutoData]
    public async Task GetRelationship_Returns_Success(long accountId)
    {
        Account account = AccountTestData.CreateAccount(accountId);

        AccountProviderLegalEntity accountProviderLegalEntitiesToAdd =
            AccountProviderLegalEntityTestData.CreateAccountProviderLegalEntity(account);

        AccountProviderLegalEntity? accountProviderLegalEntity = null;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(GetRelationship_Returns_Success)}")
        )
        {
            await context.AccountProviderLegalEntities.AddAsync(accountProviderLegalEntitiesToAdd, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            PermissionsReadRepository sut = new(context);
            accountProviderLegalEntity = await sut.GetRelationship(
                accountProviderLegalEntitiesToAdd.AccountProvider.ProviderUkprn,
                accountProviderLegalEntitiesToAdd.AccountLegalEntityId, 
                cancellationToken
            );
        }

        Assert.Multiple(() => {
            Assert.That(accountProviderLegalEntity, Is.Not.Null, "AccountProviderLegalEntity should not be null");
            Assert.That(accountProviderLegalEntity!.AccountProvider, Is.Not.Null, "AccountProvider should not be null");
            Assert.That(accountProviderLegalEntity!.AccountProvider.ProviderUkprn, Is.EqualTo(accountProviderLegalEntitiesToAdd.AccountProvider.ProviderUkprn), $"{accountProviderLegalEntity!.AccountProvider.ProviderUkprn} should equal {accountProviderLegalEntitiesToAdd.AccountProvider.ProviderUkprn}");
            Assert.That(accountProviderLegalEntity!.AccountLegalEntity, Is.Not.Null, "AccountLegalEntity should not be null");
            Assert.That(accountProviderLegalEntity!.AccountLegalEntity.Id, Is.EqualTo(accountProviderLegalEntitiesToAdd.AccountLegalEntity.Id), $"{accountProviderLegalEntity!.AccountLegalEntity.Id} should equal {accountProviderLegalEntitiesToAdd.AccountLegalEntity.Id}");
            Assert.That(accountProviderLegalEntity!.AccountLegalEntity.Deleted, Is.Null, "AccountLegalEntity should not be deleted");
            Assert.That(accountProviderLegalEntity!.Permissions.Count, Is.EqualTo(1), "One permission should exst");
        });
    }
}
