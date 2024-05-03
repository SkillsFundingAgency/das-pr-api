using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Permissions.Queries.GetPermissionsHas;
public class GetPermissionHasHandler(IPermissionsReadRespository _permissionsReadRespository) : IRequestHandler<GetPermissionsHasQuery, ValidatedResponse<GetPermissionsHasResult>>
{
    public async Task<ValidatedResponse<GetPermissionsHasResult>> Handle(GetPermissionsHasQuery query, CancellationToken cancellationToken)
    {
        var hasPermissions = await _permissionsReadRespository.GetPermissionsHas(query.Ukprn!.Value, query.PublicHashedId,
            query.Operations, cancellationToken);

        GetPermissionsHasResult result = new GetPermissionsHasResult { HasPermissions = hasPermissions };

        return new ValidatedResponse<GetPermissionsHasResult>(result);
    }
}

