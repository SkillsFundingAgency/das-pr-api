﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SFA.DAS.PR.Data.ValueConvertors;
using SFA.DAS.PR.Domain.Entities;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PR.Data.EntityConfiguration;

[ExcludeFromCodeCoverage]
public class RequestConfiguration : IEntityTypeConfiguration<Request>
{
    public void Configure(EntityTypeBuilder<Request> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(e => e.Status)
               .HasConversion(new EnumToStringConverter<RequestStatus>());

        builder.Property(e => e.RequestType)
               .HasConversion(new EnumToStringConverter<RequestType>());

        builder.HasOne(e => e.Provider)
                .WithMany(p => p.Requests)
                .HasForeignKey(e => e.Ukprn)
                .HasPrincipalKey(p => p.Ukprn);
    }
}
