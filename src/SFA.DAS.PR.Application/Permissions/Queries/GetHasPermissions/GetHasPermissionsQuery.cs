using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Permissions.Queries.GetHasPermissions;

public class GetHasPermissionsQuery : IRequest<ValidatedResponse<bool>>, IUkprnEntity, IOperationsEntity
{
    public long? Ukprn { get; set; }
    public long? AccountLegalEntityId { get; set; }
    public List<Operation> Operations { get; set; } = new List<Operation>();
}