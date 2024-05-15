using SFA.DAS.PR.Application.AccountProviders.Queries.GetPermissions;
using SFA.DAS.PR.Domain.Entities;
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
            }
        };

        return result;
    }
}
