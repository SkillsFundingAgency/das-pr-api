using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Api.RouteValues.Permissions;

public class HasPermissionRouteValues
{
    public long? Ukprn { get; set; }
    public string? PublicHashedId { get; set; }
    public List<Operation>? Operations { get; set; }
}