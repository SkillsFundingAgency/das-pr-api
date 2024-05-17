using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Permissions.Queries.GetHasPermissions;

public class GetHasPermissionsQueryHandler(IPermissionsReadRepository _permissionsReadRespository) : IRequestHandler<GetHasPermissionsQuery, ValidatedBooleanResult>
{
    public async Task<ValidatedBooleanResult> Handle(GetHasPermissionsQuery query, CancellationToken cancellationToken)
    {
        var hasPermissions = await _permissionsReadRespository.GetHasPermissions(query.Ukprn!.Value, query.AccountLegalEntityId!.Value,
            query.Operations, cancellationToken);

        return new ValidatedBooleanResult(hasPermissions);
    }
}