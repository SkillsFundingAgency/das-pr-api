using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.PR.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PR.Data.EntityConfiguration;

[ExcludeFromCodeCoverage]
public class AccountLegalEntityConfiguration : IEntityTypeConfiguration<AccountLegalEntity>
{
    public void Configure(EntityTypeBuilder<AccountLegalEntity> builder)
    {
        builder
            .HasKey(p => p.Id);

        builder
            .HasOne(a => a.Account)
            .WithMany(a => a.AccountLegalEntities)
            .HasForeignKey(a => a.AccountId);

        builder
            .HasMany(a => a.AccountProviderLegalEntities)
            .WithOne(a => a.AccountLegalEntity)
            .HasForeignKey(a => a.AccountLegalEntityId);
    }
}