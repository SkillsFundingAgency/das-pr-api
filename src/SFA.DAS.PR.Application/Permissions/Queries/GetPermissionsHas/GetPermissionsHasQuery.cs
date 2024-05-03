using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Permissions.Queries.GetPermissionsHas;
public class GetPermissionsHasQuery : IRequest<ValidatedResponse<GetPermissionsHasResult>>, IUkprnEntity, IOperationsEntity
{
    public long? Ukprn { get; set; }
    public string? PublicHashedId { get; set; }

    public List<Operation>? Operations { get; set; }
}