using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;

namespace SFA.DAS.PR.Application.Permissions.Queries.GetEmployerRelationships;

public class GetEmployerRelationshipsQuery : IRequest<ValidatedResponse<GetEmployerRelationshipsQueryResult>>
{
    public string AccountHashedId { get; set; }

    public GetEmployerRelationshipsQuery(string accountHashedId)
    {
        AccountHashedId = accountHashedId;
    }
}
