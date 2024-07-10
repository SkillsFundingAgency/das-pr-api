using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.PR.Application.Relationships.Queries.GetRelationships;

public class GetRelationshipsQueryHandler(IAccountProviderLegalEntitiesReadRepository accountProviderLegalEntitiesReadRepository, IPermissionAuditReadRepository permissionAuditReadRepository, IRequestReadRepository requestReadRepository) : IRequestHandler<GetRelationshipsQuery, ValidatedResponse<GetRelationshipsQueryResult?>>
{
    public async Task<ValidatedResponse<GetRelationshipsQueryResult?>> Handle(GetRelationshipsQuery query, CancellationToken cancellationToken)
    {
        AccountProviderLegalEntity? accountProviderLegalEntity = await accountProviderLegalEntitiesReadRepository.GetAccountProviderLegalEntityByProvider(query.Ukprn!.Value, query.AccountLegalEntityId!.Value, cancellationToken);

        if (accountProviderLegalEntity == null)
        {
            return ValidatedResponse<GetRelationshipsQueryResult?>.EmptySuccessResponse();
        }

        GetRelationshipsQueryResult result = (GetRelationshipsQueryResult)accountProviderLegalEntity;

        PermissionsAudit? permissionAudit = await permissionAuditReadRepository.GetMostRecentPermissionAudit(query.Ukprn!.Value, query.AccountLegalEntityId!.Value, cancellationToken);

        if (permissionAudit != null)
        {
            result.LastAction = Enum.Parse<PermissionAction>(permissionAudit.Action);
            result.LastActionTime = permissionAudit.Eventtime;
        }
        else
        {
            result.LastActionTime = accountProviderLegalEntity.Updated ?? accountProviderLegalEntity.Created;
        }

        Request? request = await requestReadRepository.GetRequest(query.Ukprn!.Value, query!.AccountLegalEntityId.Value, cancellationToken);

        if (request != null)
        {
            result.LastRequestType = request.RequestType;
            result.LastRequestTime = request.UpdatedDate ?? request.RequestedDate;
            result.LastRequestStatus = Enum.Parse<RequestStatus>(request.Status);
            result.LastRequestOperations = request.PermissionRequests.Select(a => (Operation)a.Operation).ToArray();
        }

        return new ValidatedResponse<GetRelationshipsQueryResult?>(result);
    }
}