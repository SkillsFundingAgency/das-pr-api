using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.PR.Application.EmployerRelationships.Queries.GetProviderRelationships;

public class ProviderRequestModel
{
    public required long Ukprn { get; set; }
    public required Guid RequestId { get; set; }
    public Operation[] Operations { get; set; } = [];

    public static implicit operator ProviderRequestModel(Request source) => new()
    {
        Ukprn = source.Ukprn,
        RequestId = source.Id,
        Operations = source.PermissionRequests.Select(a => (Operation)a.Operation).ToArray()
    };
}
