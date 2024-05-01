namespace SFA.DAS.PR.Application.AccountProviders.Queries.GetAccountProviders;

public record GetAccountProvidersQueryResult(string accountHashedId, List<AccountProviderModel> AccountProviders);