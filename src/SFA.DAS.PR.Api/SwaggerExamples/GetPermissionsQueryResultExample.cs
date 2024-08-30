using SFA.DAS.PR.Application.Permissions.Queries.GetPermissions;
using SFA.DAS.ProviderRelationships.Types.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PR.Api.SwaggerExamples;

[ExcludeFromCodeCoverage]
public class GetPermissionsQueryResultExample : IExamplesProvider<GetPermissionsQueryResult>
{
    public GetPermissionsQueryResult GetExamples()
    {
        GetPermissionsQueryResult result = new GetPermissionsQueryResult()
        {
            Operations = new List<Operation>
            {
                Operation.CreateCohort,
                Operation.Recruitment
            },
            ProviderName = "provider name",
            AccountLegalEntityName = "ALE Name"
        };

        return result;
    }
}
