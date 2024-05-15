using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Application.Permissions.Queries.GetPermissions;

public class AccountLegalEntityPermissionsModel
{
    public required string PublicHashedId { get; set; }

    public required string Name {  get; set; }

    public List<AccountProviderLegalEntityPermissionsModel> Permissions { get; set; } = [];

    public static implicit operator AccountLegalEntityPermissionsModel(AccountLegalEntity source) => new()
    {
        PublicHashedId = source.PublicHashedId,
        Name = source.Name,
        Permissions = source.AccountProviderLegalEntities.Select(a => (AccountProviderLegalEntityPermissionsModel)a).ToList()
    };
}
