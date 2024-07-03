using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Data.EntityConfiguration;

[ExcludeFromCodeCoverage]
public class PermissionsAuditConfiguration : IEntityTypeConfiguration<PermissionsAudit>
{
    public void Configure(EntityTypeBuilder<PermissionsAudit> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(a => a.Action).HasMaxLength(30);
    }
}