using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;
using SFA.DAS.PR.Api.Authorization;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PR.Api.Infrastructure;

[ExcludeFromCodeCoverage]
public class AuthorizeByPathControllerModelConvention : IControllerModelConvention
{
    private const string IntegationPathName = "IntegrationControllers";
    private const string ManagementPathName = "ManagementControllers";
    public void Apply(ControllerModel controller)
    {
        var controllerPath = controller?.ControllerType?.Namespace?.Split('.').Last();
        if(!string.IsNullOrWhiteSpace(controllerPath) && controllerPath.Equals(IntegationPathName))
        {
            controller?.Filters.Add(new AuthorizeFilter(Policies.Integration));
        }
        if (!string.IsNullOrWhiteSpace(controllerPath) && controllerPath.Equals(ManagementPathName))
        {
            controller?.Filters.Add(new AuthorizeFilter(Policies.Management));
        }
    }
}
