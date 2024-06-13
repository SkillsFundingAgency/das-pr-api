using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.PR.Domain.Convertors;
using SFA.DAS.PR.Domain.Entities;
using System.Text.Json;

namespace SFA.DAS.PR.Api.Attributes;

public class UseEnumMemberConverterAttribute : ActionFilterAttribute
{
    public override void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Result is ObjectResult objectResult)
        {
            var options = new JsonSerializerOptions
            {
                Converters =
                {
                    new JsonStringEnumMemberConverter<PermissionAction>(),
                    new JsonStringEnumMemberConverter<RequestStatus>()
                }
            };

            objectResult.Value = JsonSerializer.Serialize(objectResult.Value, options);
        }
        base.OnActionExecuted(context);
    }
}
