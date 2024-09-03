using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;

namespace SFA.DAS.PR.Application.Requests.Commands.DeclinedRequest;

public sealed class DeclinedRequestCommandHandler : IRequestHandler<DeclinedRequestCommand, ValidatedResponse<Unit>>
{
    public async Task<ValidatedResponse<Unit>> Handle(DeclinedRequestCommand command, CancellationToken cancellationToken)
    {


        return ValidatedResponse<Unit>.EmptySuccessResponse();
    }
}
