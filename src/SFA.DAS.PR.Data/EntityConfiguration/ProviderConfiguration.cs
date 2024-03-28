using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.PR.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PR.Data.EntityConfiguration;

[ExcludeFromCodeCoverage]
public class ProviderConfiguration : IEntityTypeConfiguration<Provider>
{
    public void Configure(EntityTypeBuilder<Provider> builder)
    {
        builder.HasKey(p => p.Ukprn);
    }
}