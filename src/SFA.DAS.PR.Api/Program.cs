using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.PR.Api.AppStart;

var builder = WebApplication.CreateBuilder(args);

IConfiguration configuration = builder.Configuration.LoadConfiguration(builder.Services);

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1",
            new OpenApiInfo
            {
                Title = "Provider Relationships API"
            });

        options.OperationFilter<SwaggerVersionHeaderFilter>();
    })
    .AddOptions()
    .AddLogging()
    .AddApplicationInsightsTelemetry()
    .AddServiceRegistrations()
    .AddAuthentication(configuration)
    .AddApiVersioning(opt =>
    {
        opt.ApiVersionReader = new HeaderApiVersionReader("X-Version");
        opt.DefaultApiVersion = new ApiVersion(1, 0);
    })
    .AddControllers(options =>
    {
        if (!configuration.IsLocalEnvironment())
            options.Conventions.Add(new AuthorizeControllerModelConvention(new List<string>()));
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddHealthChecks();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseAuthentication();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "SFA.DAS.AANHub.Api v1");
    options.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();
app.UseRouting();

app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = HealthCheckResponseWriter.WriteJsonResponse
    });

app.UseHealthChecks("/ping",
    new HealthCheckOptions
    {
        Predicate = _ => false,
        ResponseWriter = (context, report) =>
        {
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync("");
        }
    });

app.UseAuthorization();

app.MapControllers();

app.Run();
