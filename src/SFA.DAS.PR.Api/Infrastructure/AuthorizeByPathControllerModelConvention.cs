using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;
using SFA.DAS.PR.Api.Authorization;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PR.Api.Infrastructure;

[ExcludeFromCodeCoverage]
public class AuthorizeByPathControllerModelConvention : IControllerModelConvention
{
    private const string ReadPathName = "ReadControllers";
    private const string WritePathName = "WriteControllers";
    public void Apply(ControllerModel controller)
    {
        var controllerPath = controller?.ControllerType?.Namespace?.Split('.').Last();
        if(!string.IsNullOrWhiteSpace(controllerPath) && controllerPath.Equals(ReadPathName))
        {
            controller?.Filters.Add(new AuthorizeFilter(ApiRoles.Read));
        }
        if (!string.IsNullOrWhiteSpace(controllerPath) && controllerPath.Equals(WritePathName))
        {
            controller?.Filters.Add(new AuthorizeFilter(ApiRoles.Write));
        }
    }
}
