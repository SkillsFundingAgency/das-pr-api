using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.PR.Domain.Models;

namespace SFA.DAS.PR.Application.Requests.Queries.LookupRequests;

public class LookupRequestsQueryHandler(IRequestReadRepository _requestReadRepository) : IRequestHandler<LookupRequestsQuery, ValidatedResponse<RequestModel?>>
{
    public async Task<ValidatedResponse<RequestModel?>> Handle(LookupRequestsQuery query, CancellationToken cancellationToken)
    {
        Request? request = await _requestReadRepository.GetRequest(query.Ukprn!.Value, query!.Paye, query!.Email, [RequestStatus.New, RequestStatus.Sent], cancellationToken);

        if (request is null)
        {
            return new ValidatedResponse<RequestModel?>();
        }

        return new ValidatedResponse<RequestModel?>((RequestModel)request);
    }
}
