using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.PR.Application.AccountProviders.Queries.GetAccountProviders;

public class AccountLegalEntityModel
{
    public string? PublicHashedId { get; set; }
    public string? Name { get; set; }
    public List<Operation> Operations { get; set; } = [];

    public static implicit operator AccountLegalEntityModel(AccountLegalEntity source) => new()
    {
        PublicHashedId = source.PublicHashedId,
        Name = source.Name,
        Operations = new()
    };
}
