using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.PR.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PR.Data.EntityConfiguration;

[ExcludeFromCodeCoverage]
public class AccountProviderLegalEntityConfiguration : IEntityTypeConfiguration<AccountProviderLegalEntity>
{
    public void Configure(EntityTypeBuilder<AccountProviderLegalEntity> builder)
    {
        builder
            .HasKey(p => p.Id);

        builder
            .HasOne(p => p.AccountProvider)
            .WithMany(a => a.AccountProviderLegalEntities)
            .HasForeignKey(a => a.AccountProviderId);

        builder
            .HasOne(p => p.AccountLegalEntity)
            .WithMany(a => a.AccountProviderLegalEntities)
            .HasForeignKey(a => a.AccountLegalEntityId);

        builder
            .HasMany(p => p.Permissions)
            .WithOne(p => p.AccountProviderLegalEntity)
            .HasForeignKey(p => p.AccountProviderLegalEntityId);
    }
}