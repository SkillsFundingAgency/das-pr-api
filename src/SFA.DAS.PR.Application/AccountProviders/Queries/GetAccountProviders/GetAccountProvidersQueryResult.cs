namespace SFA.DAS.PR.Application.AccountProviders.Queries.GetAccountProviders;

public record GetAccountProvidersQueryResult(long accountId, List<AccountProviderModel> AccountProviders);