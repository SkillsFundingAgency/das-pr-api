using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Permissions.Queries.HasRelationshipWithPermission;
public class HasRelationshipWithPermissionQueryHandler(IPermissionsReadRepository permissionsReadRepository) : IRequestHandler<HasRelationshipWithPermissionQuery, ValidatedResponse<HasRelationshipWithPermissionQueryResult>>
{
    public async Task<ValidatedResponse<HasRelationshipWithPermissionQueryResult>> Handle(HasRelationshipWithPermissionQuery query, CancellationToken cancellationToken)
    {
        //bool hasRelationshipWithPermission = await permissionsReadRepository.HasPermissionWithRelationship(query.Ukprn, query.Operation, cancellationToken);

        return new ValidatedResponse<HasRelationshipWithPermissionQueryResult>(new HasRelationshipWithPermissionQueryResult(false));
    }
}