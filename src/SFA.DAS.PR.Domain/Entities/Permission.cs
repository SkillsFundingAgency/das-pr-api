using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PR.Domain.Entities;

[ExcludeFromCodeCoverage]
public class Permission
{
    public long Id { get; set; }
    public long AccountProviderLegalEntityId { get; set; }
    public Operation Operation { get; set; }
    public virtual AccountProviderLegalEntity AccountProviderLegalEntity { get; set; } = null!;
}
