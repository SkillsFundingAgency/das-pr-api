using SFA.DAS.PR.Api.RouteValues.Permissions;
using SFA.DAS.PR.Domain.Entities;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PR.Api.SwaggerExamples;

[ExcludeFromCodeCoverage]
public class HasPermissionRouteValuesExample : IExamplesProvider<HasPermissionRouteValues>
{
    public HasPermissionRouteValues GetExamples()
    {
        HasPermissionRouteValues result = new HasPermissionRouteValues
        {
            Ukprn = 12345678,
            PublicHashedId = "LegalEntityHashedId",
            Operations = new List<Operation>
            {
                Operation.CreateCohort,
                Operation.RecruitmentRequiresReview
            }
        };

        return result;
    }
}