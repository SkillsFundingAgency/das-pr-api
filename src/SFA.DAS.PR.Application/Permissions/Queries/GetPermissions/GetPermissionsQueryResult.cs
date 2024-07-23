using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.PR.Application.Permissions.Queries.GetPermissions;
public class GetPermissionsQueryResult
{
    public List<Operation> Operations { get; set; } = new();
    public string ProviderName { get; set; } = null!;
    public string AccountLegalEntityName { get; set; } = null!;
}
