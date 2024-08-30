using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Models;

namespace SFA.DAS.PR.Application.Requests.Queries.GetRequest;

public class GetRequestQuery : IRequest<ValidatedResponse<RequestModel?>>
{
    public Guid RequestId { get; set; }

    public GetRequestQuery(Guid requestId)
    {
        RequestId = requestId;
    }
}
