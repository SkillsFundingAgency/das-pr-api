using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.PR.Application.Permissions.Queries.GetPermissions;
public class GetPermissionsQueryResult
{
    public List<Operation> Operations { get; set; } = new();
    public required string ProviderName { get; set; }
    public required string AccountLegalEntityName { get; set; }
}
