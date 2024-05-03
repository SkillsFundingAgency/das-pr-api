using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Application.AccountProviderLegalEntities.Queries.GetAccountProviderLegalEntities;

public class AccountProviderLegalEntityModel
{
    public long AccountId {  get; set; }
    public string? AccountHashedId { get; set; }
    public string? AccountPublicHashedId { get; set; }
    public string? AccountName { get; set; }
    public long AccountLegalEntityId { get; set; }
    public string? AccountLegalEntityPublicHashedId { get; set; }
    public string? AccountLegalEntityName { get; set; }
    public long AccountProviderId { get; set; }

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