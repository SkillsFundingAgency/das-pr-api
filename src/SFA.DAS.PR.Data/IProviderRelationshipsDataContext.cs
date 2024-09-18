using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Data;
public interface IProviderRelationshipsDataContext
{
    DbSet<AccountLegalEntity> AccountLegalEntities { get; }
    DbSet<AccountProviderLegalEntity> AccountProviderLegalEntities { get; }
    DbSet<AccountProvider> AccountProviders { get; }
    DbSet<Account> Accounts { get; }
    DbSet<Permission> Permissions { get; }
    DbSet<Provider> Providers { get; }
    DbSet<PermissionsAudit> PermissionsAudit { get; }
    DbSet<Request> Requests { get; }
    DbSet<PermissionRequest> PermissionRequests { get; }
    DbSet<Notification> Notifications { get; }
    DbSet<ProviderRelationship> ProviderRelationships { get; }
    EntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
