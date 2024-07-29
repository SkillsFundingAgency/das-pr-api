using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.PR.Domain.Models;

namespace SFA.DAS.PR.Application.Requests.Queries.GetRequest;

public class GetRequestQueryHandler(IRequestReadRepository _requestReadRepository) : IRequestHandler<GetRequestQuery, ValidatedResponse<RequestModel?>>
{
    public async Task<ValidatedResponse<RequestModel?>> Handle(GetRequestQuery command, CancellationToken cancellationToken)
    {
        Request? request = await _requestReadRepository.GetRequest(command.RequestId, cancellationToken);

        if(request is null)
        {
            return new ValidatedResponse<RequestModel?>();
        }

        return new ValidatedResponse<RequestModel?>((RequestModel)request);
    }
}
