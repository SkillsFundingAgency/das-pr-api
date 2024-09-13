using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.PR.Domain.Entities;

public class AccountProviderLegalEntity
{
    public AccountProviderLegalEntity() { }

    public AccountProviderLegalEntity(AccountProvider accountProvider, long accountLegalEntityId, List<Operation> operations)
    {
        AccountProvider = accountProvider;
        AccountLegalEntityId = accountLegalEntityId;
        Created = DateTime.UtcNow;
        Updated = DateTime.UtcNow;
        Permissions = operations.Select(a => new Permission()
        {
            Operation = a
        }).ToList();
    }

    public long Id { get; set; }
    public long AccountProviderId { get; set; }
    public long AccountLegalEntityId { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }
    public virtual AccountLegalEntity AccountLegalEntity { get; set; } = null!;
    public virtual AccountProvider AccountProvider { get; set; } = null!;
    public virtual List<Permission> Permissions { get; set; } = new();
}
