using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Permissions.Queries.GetAllPermissionsForAccount;

public class GetAllPermissionsForAccountQueryHandler(IPermissionsReadRepository permissionsReadRepository) : IRequestHandler<GetAllPermissionsForAccountQuery, ValidatedResponse<GetAllPermissionsForAccountQueryResult>>
{
    public async Task<ValidatedResponse<GetAllPermissionsForAccountQueryResult>> Handle(GetAllPermissionsForAccountQuery query, CancellationToken cancellationToken)
    {
        Account? account = await permissionsReadRepository.GetPermissions(query.AccountHashedId, cancellationToken);

        if(account is null)
        {
            return new ValidatedResponse<GetAllPermissionsForAccountQueryResult>(new GetAllPermissionsForAccountQueryResult());
        }

        GetAllPermissionsForAccountQueryResult queryResult = new GetAllPermissionsForAccountQueryResult(account.AccountLegalEntities.Select(a => (AccountLegalEntityPermissionsModel)a).ToList());

        return new ValidatedResponse<GetAllPermissionsForAccountQueryResult>(queryResult);
    }
}