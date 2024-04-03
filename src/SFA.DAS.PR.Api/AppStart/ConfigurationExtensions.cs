using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PR.Api.AppStart;

[ExcludeFromCodeCoverage]
public static class ConfigurationExtensions
{
    public static bool IsLocalEnvironment(this IConfiguration configuration)
    {
        var environmentName = configuration["EnvironmentName"]!;
        return environmentName.Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase);
    }
}
