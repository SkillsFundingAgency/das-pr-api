using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Application.Permissions.Queries.GetPermissionsForProviderOnAccount;
public class GetPermissionsForProviderOnAccountQueryResult
{
    public List<Operation> Operations { get; set; } = new();
}
