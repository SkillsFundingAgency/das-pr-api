﻿using Microsoft.EntityFrameworkCore;
using SFA.DAS.PR.Data.EntityConfiguration;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Data;

internal class ProviderRelationshipsDataContext : DbContext, IProviderRelationshipsDataContext
{
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<AccountLegalEntity> AccountLegalEntities => Set<AccountLegalEntity>();
    public DbSet<Provider> Providers => Set<Provider>();
    public DbSet<AccountProvider> AccountProviders => Set<AccountProvider>();
    public DbSet<AccountProviderLegalEntity> AccountProviderLegalEntities => Set<AccountProviderLegalEntity>();
    public DbSet<Permission> Permissions => Set<Permission>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ProviderConfiguration).Assembly);
    }
}