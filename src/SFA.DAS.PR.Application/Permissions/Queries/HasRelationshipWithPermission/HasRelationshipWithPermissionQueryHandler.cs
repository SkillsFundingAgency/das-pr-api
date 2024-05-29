using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Permissions.Queries.HasRelationshipWithPermission;
public class HasRelationshipWithPermissionQueryHandler(IPermissionsReadRepository permissionsReadRepository) : IRequestHandler<HasRelationshipWithPermissionQuery, ValidatedResponse<bool>>
{
    public async Task<ValidatedResponse<bool>> Handle(HasRelationshipWithPermissionQuery query, CancellationToken cancellationToken)
    {
        bool hasRelationshipWithPermission = await permissionsReadRepository.HasPermissionWithRelationship(query.Ukprn!.Value, query.Operation!.Value, cancellationToken);
        return new ValidatedResponse<bool>(hasRelationshipWithPermission);
    }
}