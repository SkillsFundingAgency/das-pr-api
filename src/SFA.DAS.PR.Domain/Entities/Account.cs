using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PR.Domain.Entities;

[ExcludeFromCodeCoverage]
public class Account
{
    public long Id { get; set; }
    public string HashedId { get; set; } = null!;
    public string PublicHashedId { get; set; } = null!;
    public string Name { get; set; } = null!;
    public DateTime Created { get; set; }
    public DateTime? Updated { get; set; }

    public virtual List<AccountProvider> AccountProviders { get; set; } = new();
    public virtual List<AccountLegalEntity> AccountLegalEntities { get; set; } = new();
}
