using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;

namespace SFA.DAS.PR.Application.Permissions.Queries.GetAllPermissionsForAccount;

public class GetAllPermissionsForAccountQuery : IRequest<ValidatedResponse<GetAllPermissionsForAccountQueryResult>>
{
    public string AccountHashedId { get; set; }

    public GetAllPermissionsForAccountQuery(string accountHashedId)
    {
        AccountHashedId = accountHashedId;
    }
}
