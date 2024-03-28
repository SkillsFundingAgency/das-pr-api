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
        builder.HasKey(p => p.Id);
    }
}