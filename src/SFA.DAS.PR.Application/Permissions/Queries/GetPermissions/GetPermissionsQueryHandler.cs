using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Permissions.Queries.GetPermissions;

public class GetPermissionsQueryHandler(IPermissionsReadRepository permissionsReadRepository) : IRequestHandler<GetPermissionsQuery, ValidatedResponse<GetPermissionsQueryResult>>
{
    public async Task<ValidatedResponse<GetPermissionsQueryResult>> Handle(GetPermissionsQuery query, CancellationToken cancellationToken)
    {
        Account? account = await permissionsReadRepository.GetPermissions(query.AccountHashedId, cancellationToken);

        if(account is null)
        {
            return new ValidatedResponse<GetPermissionsQueryResult>(new GetPermissionsQueryResult());
        }

        GetPermissionsQueryResult queryResult = new GetPermissionsQueryResult(account.AccountLegalEntities.Select(a => (AccountLegalEntityPermissionsModel)a).ToList());

        return new ValidatedResponse<GetPermissionsQueryResult>(queryResult);
    }
}