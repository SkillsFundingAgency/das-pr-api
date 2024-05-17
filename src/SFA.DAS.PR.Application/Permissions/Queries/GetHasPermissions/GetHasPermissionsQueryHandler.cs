using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Permissions.Queries.GetHasPermissions;

public class GetHasPermissionsQueryHandler(IPermissionsReadRepository _permissionsReadRespository) : IRequestHandler<GetHasPermissionsQuery, ValidatedBooleanResult>
{
    public async Task<ValidatedBooleanResult> Handle(GetHasPermissionsQuery query, CancellationToken cancellationToken)
    {
        var operations = await _permissionsReadRespository.GetOperations(query.Ukprn!.Value, query.AccountLegalEntityId!.Value,
            cancellationToken);

        bool hasPermissions = operations.Count > 0 && query.Operations.TrueForAll(operation => operations.Exists(x => x == operation));

        return new ValidatedBooleanResult(hasPermissions);
    }
}