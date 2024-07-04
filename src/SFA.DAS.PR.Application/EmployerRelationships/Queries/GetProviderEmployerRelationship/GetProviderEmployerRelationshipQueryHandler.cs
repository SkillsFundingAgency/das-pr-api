using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.EmployerRelationships.Queries.GetProviderEmployerRelationship;

public class GetProviderEmployerRelationshipQueryHandler(IAccountProviderLegalEntitiesReadRepository accountProviderLegalEntitiesReadRepository, IPermissionAuditReadRepository permissionAuditReadRepository, IRequestReadRepository requestReadRepository) : IRequestHandler<GetProviderEmployerRelationshipQuery, ValidatedResponse<GetProviderEmployerRelationshipQueryResult?>>
{
    public async Task<ValidatedResponse<GetProviderEmployerRelationshipQueryResult?>> Handle(GetProviderEmployerRelationshipQuery query, CancellationToken cancellationToken)
    {
        AccountProviderLegalEntity? accountProviderLegalEntity = await accountProviderLegalEntitiesReadRepository.GetAccountProviderLegalEntityByProvider(query.Ukprn!.Value, query.AccountLegalEntityId!.Value, cancellationToken);

        if(accountProviderLegalEntity == null)
        {
            return ValidatedResponse<GetProviderEmployerRelationshipQueryResult?>.EmptySuccessResponse();
        }

        GetProviderEmployerRelationshipQueryResult result = (GetProviderEmployerRelationshipQueryResult)accountProviderLegalEntity;

        PermissionsAudit? permissionAudit = await permissionAuditReadRepository.GetMostRecentPermissionAudit(query.Ukprn!.Value, query.AccountLegalEntityId!.Value, cancellationToken);

        if(permissionAudit != null)
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
        }

        return new ValidatedResponse<GetProviderEmployerRelationshipQueryResult?>(result);
    }
}