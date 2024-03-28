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
        builder.HasKey(p => p.Id);
    }
}