using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;

namespace SFA.DAS.PR.Application.Permissions.Queries.GetPermissions;

public class GetPermissionsQuery : IRequest<ValidatedResponse<GetPermissionsQueryResult>>
{
    public string AccountHashedId { get; set; }

    public GetPermissionsQuery(string accountHashedId)
    {
        AccountHashedId = accountHashedId;
    }
}
