using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace SFA.DAS.PR.Api.Infrastructure;
public class ApiExplorerGroupingByAuthorizeAttributeConvention : IActionModelConvention
{
    public void Apply(ActionModel action)
    {
        var authorizePolicy = action.Attributes.OfType<AuthorizeAttribute>().FirstOrDefault();
        if (authorizePolicy != null)
        {
            action.ApiExplorer.GroupName = authorizePolicy.Policy?.ToString();
        }
    }
}