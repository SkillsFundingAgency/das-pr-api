using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Application.AccountProviders.Queries.GetPermissions;
public class GetPermissionsQueryResult
{
    public List<Operation> Operations { get; set; } = new();
}
