using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Permissions.Queries.GetPermissionsForProviderOnAccount;
public class GetPermissionsForProviderOnAccountQueryHandler(IPermissionsReadRepository _permissionsReadRespository) : IRequestHandler<GetPermissionsForProviderOnAccountQuery, ValidatedResponse<GetPermissionsForProviderOnAccountQueryResult>>
{
    public async Task<ValidatedResponse<GetPermissionsForProviderOnAccountQueryResult>> Handle(GetPermissionsForProviderOnAccountQuery query, CancellationToken cancellationToken)
    {
        var operations = await _permissionsReadRespository.GetOperations(query.Ukprn!.Value, query.PublicHashedId, cancellationToken);

        GetPermissionsForProviderOnAccountQueryResult result = new GetPermissionsForProviderOnAccountQueryResult { Operations = operations };

        return new ValidatedResponse<GetPermissionsForProviderOnAccountQueryResult>(result);
    }
}
