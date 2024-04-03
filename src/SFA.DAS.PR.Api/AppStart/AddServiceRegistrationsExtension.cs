using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PR.Api.AppStart;

[ExcludeFromCodeCoverage]
public static class AddServiceRegistrationsExtension
{
    public static IServiceCollection AddServiceRegistrations(this IServiceCollection services)
    {
        return services;
    }
}
