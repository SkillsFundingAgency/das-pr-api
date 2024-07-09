using System.Diagnostics.CodeAnalysis;
using Azure.Core;
using Azure.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.PR.Data.Repositories;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Data.Extensions;

[ExcludeFromCodeCoverage]
public static class AddPrDataContextExtension
{
    private static readonly string AzureResource = "https://database.windows.net/";

    private static readonly ChainedTokenCredential AzureTokenProvider = new ChainedTokenCredential(
        new ManagedIdentityCredential(),
        new AzureCliCredential(),
        new VisualStudioCodeCredential(),
        new VisualStudioCredential()
    );

    public static IServiceCollection AddPrDataContext(this IServiceCollection services, string connectionString, string environmentName)
    {
        services.AddDbContext<ProviderRelationshipsDataContext>((serviceProvider, options) =>
        {
            SqlConnection connection = null!;

            if (!environmentName.Equals("LOCAL", StringComparison.CurrentCultureIgnoreCase))
            {
                connection = new SqlConnection
                {
                    ConnectionString = connectionString,
                    AccessToken = AzureTokenProvider.GetToken(new TokenRequestContext(scopes: new string[] { AzureResource })).Token
                };
            }
            else
            {
                connection = new SqlConnection(connectionString);
            }

            options.UseSqlServer(
                connection,
                o => o.CommandTimeout((int)TimeSpan.FromMinutes(5).TotalSeconds));
        });

        services.AddTransient<IProviderRelationshipsDataContext, ProviderRelationshipsDataContext>(provider => provider.GetService<ProviderRelationshipsDataContext>()!);

        services
            .AddHealthChecks()
            .AddDbContextCheck<ProviderRelationshipsDataContext>();

        RegisterRepositories(services);

        return services;
    }

    private static void RegisterRepositories(IServiceCollection services)
    {
        services.AddTransient<IAccountLegalEntityReadRepository, AccountLegalEntityReadRepository>();
        services.AddTransient<IAccountProviderLegalEntitiesReadRepository, AccountProviderLegalEntitiesReadRepository>();
        services.AddTransient<IPermissionsReadRepository, PermissionsReadRepository>();
        services.AddTransient<IEmployerRelationshipsReadRepository, EmployerRelationshipsReadRepository>();
        services.AddTransient<IAccountProviderWriteRepository, AccountProviderWriteRepository>();
        services.AddTransient<IAccountProviderLegalEntitiesWriteRepository, AccountProviderLegalEntitiesWriteRepository>();
        services.AddTransient<IPermissionsWriteRepository, PermissionsWriteRepository>();
        services.AddTransient<IPermissionsAuditWriteRepository, PermissionsAuditWriteRepository>();
        services.AddTransient<IRequestReadRepository, RequestReadRepository>();
        services.AddTransient<IPermissionAuditReadRepository, PermissionAuditReadRepository>();
        services.AddTransient<IProviderReadRepository, ProviderReadRepository>();
        services.AddTransient<INotificationsWriteRepository, NotificationsWriteRepository>();
        services.AddTransient<IProviderRelationshipsReadRepository, ProviderRelationshipsReadRepository>();
    }
}
