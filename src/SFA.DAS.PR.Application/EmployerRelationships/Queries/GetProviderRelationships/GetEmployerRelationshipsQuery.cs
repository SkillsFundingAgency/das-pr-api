using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;

namespace SFA.DAS.PR.Application.Permissions.Queries.GetEmployerRelationships;

public class GetEmployerRelationshipsQuery : IRequest<ValidatedResponse<GetEmployerRelationshipsQueryResult>>
{
    public long AccountId { get; set; }

    public GetEmployerRelationshipsQuery(long accountId)
    {
        AccountId = accountId;
    }
}
