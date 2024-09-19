﻿using SFA.DAS.Encoding;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PR.Api.AppStart;

[ExcludeFromCodeCoverage]
public static class AddEncodingServiceExtensions
{
    public static IServiceCollection AddEncodingService(this IServiceCollection services)
    {
        services.AddSingleton<IEncodingService, EncodingService>();
        return services;
    }
}