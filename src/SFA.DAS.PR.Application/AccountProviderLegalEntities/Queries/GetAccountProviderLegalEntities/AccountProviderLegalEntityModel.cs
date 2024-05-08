using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Application.AccountProviderLegalEntities.Queries.GetAccountProviderLegalEntities;

public class AccountProviderLegalEntityModel
{
    public required long AccountId {  get; set; }
    public required string AccountHashedId { get; set; }
    public required string AccountPublicHashedId { get; set; }
    public required string AccountName { get; set; }
    public required long AccountLegalEntityId { get; set; }
    public required string AccountLegalEntityPublicHashedId { get; set; }
    public required string AccountLegalEntityName { get; set; }
    public required long AccountProviderId { get; set; }

    public static implicit operator AccountProviderLegalEntityModel(AccountProviderLegalEntity source) => new()
    {
        AccountId = source.AccountProvider.AccountId,
        AccountHashedId = source.AccountProvider.Account.HashedId,
        AccountPublicHashedId = source.AccountProvider.Account.PublicHashedId,
        AccountName = source.AccountProvider.Account.Name,
        AccountLegalEntityId = source.AccountLegalEntity.Id,
        AccountLegalEntityPublicHashedId = source.AccountLegalEntity.PublicHashedId,
        AccountLegalEntityName = source.AccountLegalEntity.Name,
        AccountProviderId = source.AccountProvider.Id
    };
}