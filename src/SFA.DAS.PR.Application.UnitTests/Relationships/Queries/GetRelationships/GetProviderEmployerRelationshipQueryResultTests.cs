using SFA.DAS.PR.Application.Relationships.Queries.GetRelationships;
using SFA.DAS.PR.Data.UnitTests.Setup;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Application.UnitTests.Relationships.Queries.GetRelationships;

public class GetProviderEmployerRelationshipQueryResultTests
{
    [Test]
    public void ImplicitOperator_ShouldConvertCorrectly()
    {
        Account account = AccountTestData.CreateAccount(1);

        AccountProviderLegalEntity accountProviderLegalEntity = AccountProviderLegalEntityTestData
            .CreateAccountProviderLegalEntity(account);

        GetRelationshipsQueryResult result = accountProviderLegalEntity;

        Assert.Multiple(() =>
        {
            Assert.That(accountProviderLegalEntity.AccountLegalEntityId, Is.EqualTo(result.AccountLegalEntityId));
            Assert.That(accountProviderLegalEntity.AccountLegalEntity.PublicHashedId, Is.EqualTo(result.AccountLegalEntitypublicHashedId));
            Assert.That(accountProviderLegalEntity.AccountLegalEntity.Name, Is.EqualTo(result.AccountLegalEntityName));
            Assert.That(accountProviderLegalEntity.AccountLegalEntity.AccountId, Is.EqualTo(result.AccountId));
            Assert.That(accountProviderLegalEntity.AccountProvider.ProviderUkprn, Is.EqualTo(result.Ukprn));
            Assert.That(accountProviderLegalEntity.Permissions.Select(a => a.Operation).ToArray(), Is.EqualTo(result.Operations));
        });
    }
}
