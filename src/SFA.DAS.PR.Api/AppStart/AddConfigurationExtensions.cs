using System.Text.Json;
using SFA.DAS.Encoding;
using SFA.DAS.PR.Api.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PR.Api.AppStart;

[ExcludeFromCodeCoverage]
public static class AddConfigurationExtensions
{
    public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var encodingsConfiguration = configuration.GetSection(ConfigurationKeys.EncodingConfig).Value;

        var encodingConfig = JsonSerializer.Deserialize<EncodingConfig>(encodingsConfiguration!);
        services.AddSingleton(encodingConfig!);

        return services;
    }
}
