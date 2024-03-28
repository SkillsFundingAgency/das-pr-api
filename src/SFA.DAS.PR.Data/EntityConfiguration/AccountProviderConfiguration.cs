using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Data.EntityConfiguration;

public class AccountProviderConfiguration : IEntityTypeConfiguration<AccountProvider>
{
    public void Configure(EntityTypeBuilder<AccountProvider> builder)
    {
        builder
            .HasOne(ap => ap.Provider)
            .WithMany(p => p.AccountProviders)
            .HasForeignKey(ap => ap.ProviderUkprn);
    }
}
