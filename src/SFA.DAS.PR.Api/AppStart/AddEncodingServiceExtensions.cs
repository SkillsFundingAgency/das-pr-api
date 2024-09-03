using SFA.DAS.Encoding;

namespace SFA.DAS.PR.Api.AppStart;
public static class AddEncodingServiceExtensions
{
    public static IServiceCollection AddEncodingService(this IServiceCollection services)
    {
        services.AddSingleton<IEncodingService, EncodingService>();
        return services;
    }
}