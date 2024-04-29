using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;
using SFA.DAS.PR.Api.Authorization;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PR.Api.Infrastructure;

[ExcludeFromCodeCoverage]
public class AuthorizeByPathControllerModelConvention : IControllerModelConvention
{
    private const string PathName = "ReadControllers";
    public void Apply(ControllerModel controller)
    {
        var controllerPath = controller?.ControllerType?.Namespace?.Split('.').Last();
        if(!string.IsNullOrEmpty(controllerPath) && controllerPath.Equals(PathName))
        {
            controller?.Filters.Add(new AuthorizeFilter(ApiRoles.Read));
        }
    }
}
