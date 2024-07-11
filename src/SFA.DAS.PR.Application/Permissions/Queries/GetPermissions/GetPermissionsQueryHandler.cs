using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.PR.Application.Permissions.Queries.GetPermissions;
public class GetPermissionsQueryHandler(IPermissionsReadRepository _permissionsReadRespository) : IRequestHandler<GetPermissionsQuery, ValidatedResponse<GetPermissionsQueryResult?>>
{
    public async Task<ValidatedResponse<GetPermissionsQueryResult?>> Handle(GetPermissionsQuery query, CancellationToken cancellationToken)
    {
        AccountProviderLegalEntity? accountProviderLegalEntity = await _permissionsReadRespository.GetRelationship(query.Ukprn!.Value, query.accountLegalEntityId!.Value, cancellationToken);

        if(accountProviderLegalEntity is null)
        {
            return new ValidatedResponse<GetPermissionsQueryResult?>();
        }

        List<Operation> operations = accountProviderLegalEntity.Permissions.Select(a => a.Operation).ToList();

        return new ValidatedResponse<GetPermissionsQueryResult?>(new GetPermissionsQueryResult() { Operations = operations });
    }
}
