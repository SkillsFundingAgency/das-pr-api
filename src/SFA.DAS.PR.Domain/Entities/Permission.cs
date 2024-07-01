namespace SFA.DAS.PR.Domain.Entities;
using SFA.DAS.ProviderRelationships.Types.Models;

public class Permission
{
    public long Id { get; set; }
    public long AccountProviderLegalEntityId { get; set; }
    public Operation Operation { get; set; }
    public virtual AccountProviderLegalEntity AccountProviderLegalEntity { get; set; } = null!;
}
