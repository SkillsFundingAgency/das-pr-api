using System.Diagnostics.CodeAnalysis;
using SFA.DAS.PR.Application.Permissions.Queries.GetHasPermissions;
using SFA.DAS.ProviderRelationships.Types.Models;
using Swashbuckle.AspNetCore.Filters;

namespace SFA.DAS.PR.Api.SwaggerExamples;

[ExcludeFromCodeCoverage]
public class GetHasPermissionsQueryExample : IExamplesProvider<GetHasPermissionsQuery>
{
    public GetHasPermissionsQuery GetExamples()
    {
        GetHasPermissionsQuery result = new GetHasPermissionsQuery
        {
            Ukprn = 12345678,
            AccountLegalEntityId = 123456567,
            Operations = new List<Operation> { Operation.CreateCohort }
        };

        return result;
    }
}
