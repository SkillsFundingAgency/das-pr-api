using SFA.DAS.PR.Application.Relationships.Queries.GetRelationships;
using SFA.DAS.PR.Data.UnitTests.Setup;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Application.UnitTests.Relationships.Queries.GetRelationships;

public class GetRelationshipsQueryResultTests
{
    [Test]
    public void ImplicitOperator_ShouldConvertCorrectly()
    {
        Account account = AccountTestData.CreateAccount(1);

        AccountProviderLegalEntity accountProviderLegalEntity = AccountProviderLegalEntityTestData
            .CreateAccountProviderLegalEntity(account);

        GetRelationshipsQueryResult sut = accountProviderLegalEntity;

        Assert.Multiple(() =>
        {
            Assert.That(accountProviderLegalEntity.AccountLegalEntityId, Is.EqualTo(sut.AccountLegalEntityId));
            Assert.That(accountProviderLegalEntity.AccountLegalEntity.PublicHashedId, Is.EqualTo(sut.AccountLegalEntitypublicHashedId));
            Assert.That(accountProviderLegalEntity.AccountLegalEntity.Name, Is.EqualTo(sut.AccountLegalEntityName));
            Assert.That(accountProviderLegalEntity.AccountLegalEntity.AccountId, Is.EqualTo(sut.AccountId));
            Assert.That(accountProviderLegalEntity.AccountProvider.ProviderUkprn, Is.EqualTo(sut.Ukprn));
            Assert.That(accountProviderLegalEntity.Permissions.Select(a => a.Operation).ToArray(), Is.EqualTo(sut.Operations));
        });
    }
}
