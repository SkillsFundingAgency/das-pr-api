using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Data.EntityConfiguration;

public class ProviderRelationshipsConfiguration : IEntityTypeConfiguration<ProviderRelationship>
{
    public void Configure(EntityTypeBuilder<ProviderRelationship> builder)
    {
        builder
            .HasNoKey()
            .ToView("ProviderRelationships");

        builder
            .Property(p => p.HasCreateCohortPermission)
            .HasColumnType("bit");

        builder
            .Property(p => p.HasCreateAdvertPermission)
            .HasColumnType("bit");

        builder
            .Property(p => p.HasCreateAdvertWithReviewPermission)
            .HasColumnType("bit");
    }
}
