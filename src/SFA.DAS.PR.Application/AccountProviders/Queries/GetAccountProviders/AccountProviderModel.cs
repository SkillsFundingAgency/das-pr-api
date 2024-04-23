using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Application.AccountProviders.Queries.GetAccountProviders;

public class AccountProviderModel
{
    public long Id { get; set; }
    public long ProviderUkprn { get; set; }
    public string? ProviderName { get; set; }
    public List<AccountLegalEntityModel> AccountLegalEntities { get; set; } = [];

    public static implicit operator AccountProviderModel(AccountProvider source) => new()
    {
        Id = source.Id,
        ProviderUkprn = source.ProviderUkprn,
        ProviderName = source.Provider.Name,
        AccountLegalEntities = source.AccountProviderLegalEntities.Select(a => (AccountLegalEntityModel)a.AccountLegalEntity).ToList()
    };
}
