using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.OpenApi.Models;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.PR.Api.AppStart;
using SFA.DAS.PR.Data.Extensions;
using System.Text.Json.Serialization;
using SFA.DAS.PR.Application.Extensions;

var builder = WebApplication.CreateBuilder(args);

IConfiguration _configuration = builder.Configuration.LoadConfiguration();

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
        if (!_configuration.IsLocalEnvironment())
            options.Conventions.Add(new AuthorizeControllerModelConvention(new List<string>()));
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddPrDataContext(_configuration["ApplicationSettings:DbConnectionString"]!, _configuration["EnvironmentName"]!);
builder.Services.AddApplicationRegistrations();

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
