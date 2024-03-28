using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Api.Configuration;

public class AccountLegalEntityConfiguration : IEntityTypeConfiguration<AccountLegalEntity>
{
    public void Configure(EntityTypeBuilder<AccountLegalEntity> builder)
    {
        builder.HasQueryFilter(ale => ale.Deleted == null);
    }
}
