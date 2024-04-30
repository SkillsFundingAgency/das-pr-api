using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Options;
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
        if(IsEnvironmentLocalOrDev)
        {
            options.Filters.Add(new AllowAnonymousFilter());
        }
        options.Conventions.Add(new ApiExplorerGroupingByAuthorizeAttributeConvention());
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(Policies.Management, new OpenApiInfo { Title = "Management", Version = "v1" });
    options.SwaggerDoc(Policies.Integration, new OpenApiInfo { Title = "Integration", Version = "v1" });
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
        {Policies.Integration, Policies.Integration},
        {Policies.Management, Policies.Management}
    });
}
else
{
    builder.Services.AddAuthorization(options =>
    {
        foreach (var policyName in new string[] { Policies.Integration, Policies.Management })
        {
            options.AddPolicy(policyName, policy =>
            {
                policy.Requirements.Add(new NoneRequirement());
            });
        }
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
    options.SwaggerEndpoint($"/swagger/{Policies.Integration}/swagger.json", Policies.Integration);
    options.SwaggerEndpoint($"/swagger/{Policies.Management}/swagger.json", Policies.Management);
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

if(IsEnvironmentLocalOrDev)
{
    app.MapControllers().AllowAnonymous();
}
else
{
    app.MapControllers();
}

app.Run();
