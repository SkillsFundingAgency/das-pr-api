﻿using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using SFA.DAS.PR.Data.EntityConfiguration;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Data;

[ExcludeFromCodeCoverage]
public class ProviderRelationshipsDataContext : DbContext, IProviderRelationshipsDataContext
{
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<AccountLegalEntity> AccountLegalEntities => Set<AccountLegalEntity>();
    public DbSet<Provider> Providers => Set<Provider>();
    public DbSet<AccountProvider> AccountProviders => Set<AccountProvider>();
    public DbSet<AccountProviderLegalEntity> AccountProviderLegalEntities => Set<AccountProviderLegalEntity>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<PermissionsAudit> PermissionsAudit => Set<PermissionsAudit>();
    public DbSet<Request> Requests => Set<Request>();
    public DbSet<PermissionRequest> PermissionRequests => Set<PermissionRequest>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<ProviderRelationship> ProviderRelationships => Set<ProviderRelationship>();

    public ProviderRelationshipsDataContext(DbContextOptions<ProviderRelationshipsDataContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProviderConfiguration).Assembly);
    }
}
