using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;
using System.Text.Json.Serialization;

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
                    new JsonStringEnumConverter()
                },
                WriteIndented = true
            };

            objectResult.Value = JsonSerializer.Serialize(objectResult.Value, options);
        }
        base.OnActionExecuted(context);
    }
}