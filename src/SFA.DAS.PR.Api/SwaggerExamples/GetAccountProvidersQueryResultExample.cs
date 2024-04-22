using SFA.DAS.PR.Application.AccountProviders.Queries.GetAccountProviders;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PR.Api.SwaggerExamples
{
    [ExcludeFromCodeCoverage]
    public class GetAccountProvidersQueryResultExample : IExamplesProvider<GetAccountProvidersQueryResult>
    {
        public GetAccountProvidersQueryResult GetExamples()
        {
            GetAccountProvidersQueryResult result = new(0, new List<AccountProviderModel>());

            return result;
        }
    }
}
