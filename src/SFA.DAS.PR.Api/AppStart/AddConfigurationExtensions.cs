using Newtonsoft.Json;
using SFA.DAS.Encoding;
using SFA.DAS.PR.Api.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PR.Api.AppStart;

[ExcludeFromCodeCoverage]
public static class AddConfigurationExtensions
{
    public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var encodingConfig = new EncodingConfig();
        configuration.GetSection(nameof(ConfigurationKeys.EncodingConfig)).Bind(encodingConfig);

        services.AddSingleton(encodingConfig);

        return services;
    }
}
