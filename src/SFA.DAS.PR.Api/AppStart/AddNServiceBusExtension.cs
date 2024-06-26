using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace SFA.DAS.PR.Api.AppStart;

[ExcludeFromCodeCoverage]
public static class AddNServiceBusExtension
{
    public static IServiceCollection AddNServicBus(this IServiceCollection services, IConfiguration configuration)
    {
        var endpointConfiguration = new EndpointConfiguration("SFA.DAS.PR.Api");
        var transport = endpointConfiguration.UseTransport<AzureServiceBusTransport>();
        transport.ConnectionString(configuration["AzureWebJobsServiceBus"]);
        endpointConfiguration.SendOnly();
        endpointConfiguration.UseSerialization<NewtonsoftJsonSerializer>();
        endpointConfiguration.Conventions()
            .DefiningCommandsAs(t => Regex.IsMatch(t.Name, "Command(V\\d+)?$"))
            .DefiningEventsAs(t => Regex.IsMatch(t.Name, "Event(V\\d+)?$"));

        var endpointInstance = NServiceBus.Endpoint.Start(endpointConfiguration).GetAwaiter().GetResult();
        services.AddSingleton(endpointInstance);
        services.AddSingleton<IMessageSession>(endpointInstance);
        return services;
    }
}
