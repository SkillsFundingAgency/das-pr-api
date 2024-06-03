using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Application.AccountProviders.Queries.GetAccountProviders;

public class AccountProviderModel
{
    public long Ukprn { get; set; }
    public string? ProviderName { get; set; }
    public List<AccountLegalEntityModel> AccountLegalEntities { get; set; } = [];

    public static implicit operator AccountProviderModel(AccountProvider source) => new()
    {
        Ukprn = source.ProviderUkprn,
        ProviderName = source.Provider.Name,
        AccountLegalEntities = new()
    };

    public static List<AccountProviderModel> BuildAccountProviderModels(List<AccountLegalEntity> legalEntities)
    {
        IEnumerable<AccountProvider> accountProviders = legalEntities.Select(a => a.Account)
                                                                     .SelectMany(a => a.AccountProviders)
                                                                     .Distinct();

        List<AccountProviderModel> providerModels = new();

        foreach (AccountProvider accountProvider in accountProviders)
        {
            AccountProviderModel providerModel = accountProvider;

            foreach (AccountLegalEntity legalEntity in legalEntities)
            {
                AccountLegalEntityModel accountLegalEntityModel = legalEntity;
                accountLegalEntityModel.Operations = legalEntity.AccountProviderLegalEntities.Where(a =>
                        a.AccountProviderId == accountProvider.Id).SelectMany(a => a.Permissions).Select(a => a.Operation).ToList();

                providerModel.AccountLegalEntities.Add(accountLegalEntityModel);
            }

            providerModels.Add(providerModel);
        }

        return providerModels;
    }
}
