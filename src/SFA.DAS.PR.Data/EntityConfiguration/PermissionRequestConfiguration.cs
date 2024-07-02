using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PR.Data.EntityConfiguration;

[ExcludeFromCodeCoverage]
public class PermissionRequestConfiguration : IEntityTypeConfiguration<PermissionRequest>
{
    public void Configure(EntityTypeBuilder<PermissionRequest> builder)
    {
        builder.HasKey(p => p.Id);
    }
}