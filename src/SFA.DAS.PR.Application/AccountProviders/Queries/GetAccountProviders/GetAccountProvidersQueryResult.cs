namespace SFA.DAS.PR.Application.AccountProviders.Queries.GetAccountProviders;

public class GetAccountProvidersQueryResult(long accountId, List<AccountProviderModel> accountProviders)
{
    public long AccountId { get; set; } = accountId;
    public List<AccountProviderModel> AccountProviders { get; set; } = accountProviders;
}