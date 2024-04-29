namespace SFA.DAS.PR.Application.AccountProviders.Queries.GetAccountProviders;

public record GetAccountProvidersQueryResult(long AccountId, List<AccountProviderModel> AccountProviders);