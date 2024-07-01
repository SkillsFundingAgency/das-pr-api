using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.PR.Application.Permissions.Queries.HasRelationshipWithPermission;

public class HasRelationshipWithPermissionQuery : IRequest<ValidatedResponse<bool>>, IUkprnEntity, IOperationEntity
{
    public long? Ukprn { get; set; }
    public required Operation? Operation { get; set; }
}