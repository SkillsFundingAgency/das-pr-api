using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.PR.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PR.Data.EntityConfiguration;

[ExcludeFromCodeCoverage]
public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(p => p.Id);

        builder
            .HasMany(a => a.AccountProviders)
            .WithOne(a => a.Account)
            .HasForeignKey(a => a.AccountId);

        builder
            .HasMany(a => a.AccountLegalEntities)
            .WithOne(a => a.Account)
            .HasForeignKey(a => a.AccountId);
    }
}