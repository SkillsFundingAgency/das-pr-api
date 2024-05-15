using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.AccountProviders.Queries.GetPermissions;
public class GetPermissionsQueryHandler(IPermissionsReadRepository _permissionsReadRespository) : IRequestHandler<GetPermissionsQuery, ValidatedResponse<GetPermissionsQueryResult>>
{
    public async Task<ValidatedResponse<GetPermissionsQueryResult>> Handle(GetPermissionsQuery query, CancellationToken cancellationToken)
    {
        var operations = await _permissionsReadRespository.GetOperations(query.Ukprn!.Value, query.PublicHashedId, cancellationToken);

        GetPermissionsQueryResult result = new GetPermissionsQueryResult { Operations = operations };

        return new ValidatedResponse<GetPermissionsQueryResult>(result);
    }
}
