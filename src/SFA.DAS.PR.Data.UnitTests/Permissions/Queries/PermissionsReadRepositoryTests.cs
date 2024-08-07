﻿using NUnit.Framework;
using SFA.DAS.PR.Data.Repositories;
using SFA.DAS.PR.Data.UnitTests.InMemoryDatabases;
using SFA.DAS.PR.Data.UnitTests.Setup;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.PR.Data.UnitTests.Permissions.Queries;
public class PermissionsReadRepositoryTests
{
    private readonly CancellationToken _cancellationToken = CancellationToken.None;

    [Test]
    public async Task AccountProviderLegalEntities_Get_Operations()
    {
        Account account = AccountTestData.CreateAccount(1004);

        List<AccountProviderLegalEntity> accountProviderLegalEntitiesToAdd =
            AccountProviderLegalEntityTestData.CreateAccountProviderLegalEntitiesWithPermissions(account);

        long ukprn = accountProviderLegalEntitiesToAdd[0].AccountProvider.ProviderUkprn;
        long accountLegalEntityId = accountProviderLegalEntitiesToAdd[0].AccountLegalEntity.Id;

        List<Operation> expectedOperations;
        var actualOperations = accountProviderLegalEntitiesToAdd[0].Permissions.Select(x => x.Operation).ToList();

        using (var context = InMemoryProviderRelationshipsDataContext.CreateInMemoryContext(
                   contextName: $"{nameof(InMemoryProviderRelationshipsDataContext)}_{nameof(AccountProviderLegalEntities_Get_Operations)}")
              )
        {
            await context.AddAsync(account);
            await context.AddRangeAsync(accountProviderLegalEntitiesToAdd);
            await context.SaveChangesAsync(_cancellationToken);

            PermissionsReadRepository sut = new(context);

            expectedOperations = await sut.GetOperations(ukprn, accountLegalEntityId, _cancellationToken);
        }

        Assert.That(expectedOperations, Is.EqualTo(actualOperations));
    }
}
