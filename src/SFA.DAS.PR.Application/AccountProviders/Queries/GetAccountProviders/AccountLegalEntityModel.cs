using SFA.DAS.PR.Domain.Entities;

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
        Operations = source.AccountProviderLegalEntities.SelectMany(a => a.Permissions).Select(a => a.Operation).ToList()
    };
}
