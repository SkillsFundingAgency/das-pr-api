using Microsoft.AspNetCore.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SFA.DAS.PR.Api.Attributes;

[ExcludeFromCodeCoverage]
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

            var jsonString = JsonSerializer.Serialize(objectResult.Value, options);
            objectResult.Value = JsonSerializer.Deserialize<object>(jsonString, options);
        }
        base.OnActionExecuted(context);
    }
}