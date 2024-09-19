using AutoFixture;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SFA.DAS.PR.Data.Repositories;
using SFA.DAS.PR.Data.UnitTests.InMemoryDatabases;
using SFA.DAS.PR.Data.UnitTests.Setup;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Data.UnitTests.Repositories;

public sealed class AccountLegalEntityWriteRepositoryTests
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
    public async Task AccountLegalEntityWriteRepository_CreateAccountLegalEntity_Valid()
    {
        AccountLegalEntity accountLegalEntity = AccountLegalEntityTestData.Create();

        AccountLegalEntity? result = null;

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
            $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(AccountLegalEntityWriteRepository_CreateAccountLegalEntity_Valid)}")
        )
        {
            AccountLegalEntityWriteRepository sut = new(context);

            await sut.CreateAccountLegalEntity(accountLegalEntity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            result = await context.AccountLegalEntities.FirstOrDefaultAsync(a => a.Id == accountLegalEntity.Id, cancellationToken);
        }

        Assert.That(result, Is.Not.Null);
    }
}
