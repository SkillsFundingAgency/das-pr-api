using System.Diagnostics.CodeAnalysis;
using SFA.DAS.Api.Common.AppStart;
using SFA.DAS.Api.Common.Configuration;
using SFA.DAS.PR.Api.Authorization;

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
                {Policies.Integration, Policies.Integration},
                {Policies.Management, Policies.Management}
            };

            services.AddAuthentication(azureAdConfiguration, policies);
        }
        else
        {
            services.AddAuthorization(options =>
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


        return services;
    }

    public static bool IsLocalEnvironment(IConfiguration configuration)
    {
        var environmentName = configuration["EnvironmentName"]!;
        return environmentName.Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase);

    }
}
