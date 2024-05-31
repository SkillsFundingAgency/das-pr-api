using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Permissions.Queries.GetPermissions;
public class GetPermissionsQueryHandler(IPermissionsReadRepository _permissionsReadRespository) : IRequestHandler<GetPermissionsQuery, ValidatedResponse<GetPermissionsQueryResult>>
{
    public async Task<ValidatedResponse<GetPermissionsQueryResult>> Handle(GetPermissionsQuery query, CancellationToken cancellationToken)
    {
        var operations = await _permissionsReadRespository.GetOperations(query.Ukprn!.Value, query.accountLegalEntityId!.Value, cancellationToken);

        GetPermissionsQueryResult result = new GetPermissionsQueryResult { Operations = operations };

        return new ValidatedResponse<GetPermissionsQueryResult>(result);
    }
}
