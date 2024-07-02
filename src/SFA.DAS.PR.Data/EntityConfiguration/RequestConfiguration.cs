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

        builder.Property(a => a.RequestType).HasMaxLength(10);

        builder.Property(a => a.ProviderUserFullName).HasMaxLength(255);

        builder.Property(a => a.RequestedBy).HasMaxLength(255);

        builder.Property(a => a.EmployerOrganisationName).HasMaxLength(250);

        builder.Property(a => a.EmployerContactFirstName).HasMaxLength(200);

        builder.Property(a => a.EmployerContactLastName).HasMaxLength(200);

        builder.Property(a => a.EmployerContactEmail).HasMaxLength(255);

        builder.Property(a => a.EmployerPAYE).HasMaxLength(16);

        builder.Property(a => a.EmployerAORN).HasMaxLength(25);
    }
}