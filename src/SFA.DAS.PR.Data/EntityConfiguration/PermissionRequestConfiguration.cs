using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Data.EntityConfiguration;

[ExcludeFromCodeCoverage]
public class PermissionRequestConfiguration : IEntityTypeConfiguration<PermissionRequest>
{
    public void Configure(EntityTypeBuilder<PermissionRequest> builder)
    {
        builder.HasKey(p => p.Id);
    }
}