using SFA.DAS.PR.Application.Permissions.Queries.GetPermissionsForProviderOnAccount;
using SFA.DAS.PR.Domain.Entities;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PR.Api.SwaggerExamples;

[ExcludeFromCodeCoverage]
public class GetPermissionsForProviderOnAccountQueryResultExample : IExamplesProvider<GetPermissionsForProviderOnAccountQueryResult>
{
    public GetPermissionsForProviderOnAccountQueryResult GetExamples()
    {
        GetPermissionsForProviderOnAccountQueryResult result = new GetPermissionsForProviderOnAccountQueryResult()
        {
            Operations = new List<Operation>
            {
                Operation.CreateCohort,
                Operation.Recruitment
            }
        };

        return result;
    }
}
