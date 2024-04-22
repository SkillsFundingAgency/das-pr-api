using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Application.AccountProviders.Queries.GetAccountProviders;

public class AccountLegalEntityModel
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public bool HadPermissions { get; set; }
    public List<Operation> Operations { get; set; } = [];

    public static implicit operator AccountLegalEntityModel(AccountLegalEntity source) => new()
    {
        Id = source.Id,
        Name = source.Name,
        HadPermissions = false,
        Operations = source.AccountProviderLegalEntities.SelectMany(a => a.Permissions).Select(a => a.Operation).ToList()
    };
}
