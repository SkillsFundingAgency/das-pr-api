using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Data.EntityConfiguration;

[ExcludeFromCodeCoverage]
public class RequestConfiguration : IEntityTypeConfiguration<Request>
{
    public void Configure(EntityTypeBuilder<Request> builder)
    {
        builder.HasKey(p => p.Id);

        builder.HasOne(e => e.Provider)
                .WithMany(p => p.Requests)
                .HasForeignKey(e => e.Ukprn)
                .HasPrincipalKey(p => p.Ukprn);
    }
}
