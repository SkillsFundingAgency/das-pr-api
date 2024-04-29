using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.PR.Api;
using SFA.DAS.PR.Api.AppStart;
using SFA.DAS.PR.Api.Authorization;
using SFA.DAS.PR.Api.Infrastructure;
using SFA.DAS.PR.Application.Extensions;
using SFA.DAS.PR.Data.Extensions;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

IConfiguration _configuration = builder.Configuration.LoadConfiguration();

bool IsEnvironmentLocalOrDev = _configuration.IsLocalEnvironment();

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
    .AddAuthentication(_configuration)
    .AddApiVersioning(opt =>
    {
        opt.ApiVersionReader = new HeaderApiVersionReader("X-Version");
        opt.DefaultApiVersion = new ApiVersion(1, 0);
    })
    .AddControllers(options =>
    {
        if (!IsEnvironmentLocalOrDev)
            options.Conventions.Add(new AuthorizeByPathControllerModelConvention());

    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddPrDataContext(_configuration["ApplicationSettings:DbConnectionString"]!, _configuration["EnvironmentName"]!);
builder.Services.AddApplicationRegistrations();

if (!IsEnvironmentLocalOrDev)
{
    var azureAdConfiguration = _configuration
        .GetSection("AzureAd")
        .Get<AzureActiveDirectoryConfiguration>();

    builder.Services.AddAuthentication(azureAdConfiguration, new()
    {
        {ApiRoles.Read, ApiRoles.Read},
        {ApiRoles.Write, ApiRoles.Write}
    });
}

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
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "SFA.DAS.PR.Api V1");
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
