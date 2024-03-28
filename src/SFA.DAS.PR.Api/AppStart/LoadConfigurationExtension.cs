using System.Diagnostics.CodeAnalysis;
using SFA.DAS.Configuration.AzureTableStorage;

namespace SFA.DAS.PR.Api.AppStart;

[ExcludeFromCodeCoverage]
public static class LoadConfigurationExtension
{
    public static IConfiguration LoadConfiguration(this IConfiguration config)
    {
        var configBuilder = new ConfigurationBuilder()
            .AddConfiguration(config)
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddEnvironmentVariables();

        configBuilder.AddAzureTableStorage(options =>
        {
            options.ConfigurationKeys = config["ConfigNames"]!.Split(",");
            options.StorageConnectionString = config["ConfigurationStorageConnectionString"];
            options.EnvironmentName = config["EnvironmentName"];
            options.PreFixConfigurationKeys = false;
        });

        var configuration = configBuilder.Build();

        return configuration;
    }
}