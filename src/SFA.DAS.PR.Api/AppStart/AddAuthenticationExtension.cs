using System.Diagnostics.CodeAnalysis;
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.Api.Common.Configuration;

namespace SFA.DAS.PR.Api.AppStart;

[ExcludeFromCodeCoverage]
public static class AddAuthenticationExtension
{
    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        if (!IsLocalEnvironment(configuration))
        {
            var azureAdConfiguration = configuration
                .GetSection("AzureAd")
                .Get<AzureActiveDirectoryConfiguration>()!;

            var policies = new Dictionary<string, string>
            {
                {
                    "Default", "Default"
                }
            };

            services.AddAuthentication(azureAdConfiguration, policies);
        }

        return services;
    }

    public static bool IsLocalEnvironment(IConfiguration configuration)
    {
        var environmentName = configuration["EnvironmentName"]!;
        return environmentName.Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase);

    }
}
