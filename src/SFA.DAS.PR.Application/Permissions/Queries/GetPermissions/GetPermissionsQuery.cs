using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Permissions.Queries.GetPermissions;
public class GetPermissionsQuery : IRequest<ValidatedResponse<GetPermissionsQueryResult?>>, IUkprnEntity
{
    public long? Ukprn { get; set; }
    public long? accountLegalEntityId { get; set; }
}