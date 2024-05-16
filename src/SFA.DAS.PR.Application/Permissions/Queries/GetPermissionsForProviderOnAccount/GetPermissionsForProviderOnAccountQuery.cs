using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Permissions.Queries.GetPermissionsForProviderOnAccount;
public class GetPermissionsForProviderOnAccountQuery : IRequest<ValidatedResponse<GetPermissionsForProviderOnAccountQueryResult>>, IUkprnEntity
{
    public long? Ukprn { get; set; }
    public string? PublicHashedId { get; set; }
}
