using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Permissions.Queries.GetEmployerRelationships;

public class GetEmployerRelationshipsQuery : IRequest<ValidatedResponse<GetEmployerRelationshipsQueryResult>>, IUkprnEntity
{
    public string AccountHashedId { get; set; }

    public long? Ukprn { get; set; }

    public string? AccountlegalentityPublicHashedId { get; set; }

    public GetEmployerRelationshipsQuery(string accountHashedId, long? ukprn = null, string? accountlegalentityPublicHashedId = null)
    {
        AccountHashedId = accountHashedId;
        Ukprn = ukprn;
        AccountlegalentityPublicHashedId = accountlegalentityPublicHashedId;
    }
}
