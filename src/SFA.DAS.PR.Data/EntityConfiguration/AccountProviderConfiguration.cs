using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.PR.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PR.Data.EntityConfiguration;

[ExcludeFromCodeCoverage]
public class AccountProviderConfiguration : IEntityTypeConfiguration<AccountProvider>
{
    public void Configure(EntityTypeBuilder<AccountProvider> builder)
    {
        builder
            .HasOne(ap => ap.Provider)
            .WithMany(p => p.AccountProviders)
            .HasForeignKey(ap => ap.ProviderUkprn);

        builder
            .HasOne(ap => ap.Account)
            .WithMany(a => a.AccountProviders)
            .HasForeignKey(a => a.AccountId);

        builder
            .HasMany(ap => ap.AccountProviderLegalEntities)
            .WithOne(aple => aple.AccountProvider)
            .HasForeignKey(a => a.AccountProviderId);
    }
}
