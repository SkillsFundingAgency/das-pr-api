using SFA.DAS.PR.Application.AccountProviderLegalEntities.Queries.GetAccountProviderLegalEntities;
using Swashbuckle.AspNetCore.Filters;

namespace SFA.DAS.PR.Api.SwaggerExamples;
public class GetAccountProviderLegalEntitiesQueryResultExample : IExamplesProvider<GetAccountProviderLegalEntitiesQueryResult>
{
    public GetAccountProviderLegalEntitiesQueryResult GetExamples()
    {
        GetAccountProviderLegalEntitiesQueryResult result = new(
            new List<AccountProviderLegalEntityModel>()
            {
                new AccountProviderLegalEntityModel()
                {
                    AccountId = 38552,
                    AccountHashedId = "VLLRPD",
                    AccountPublicHashedId = "LX8K6M",
                    AccountName = "AYO CONSULTING LIMITED",
                    AccountLegalEntityId = 33203,
                    AccountLegalEntityPublicHashedId = "X94R86",
                    AccountLegalEntityName = "KT & A LIMITED",
                    AccountProviderId = 1037
                }
            }
        );
        return result;
    }
}