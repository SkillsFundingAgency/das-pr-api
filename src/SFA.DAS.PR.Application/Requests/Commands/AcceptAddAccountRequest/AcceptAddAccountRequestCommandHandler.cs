using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;

namespace SFA.DAS.PR.Application.Requests.Commands.AcceptAddAccountRequest;

public sealed class AcceptAddAccountRequestCommandHandler : IRequestHandler<AcceptAddAccountRequestCommand, ValidatedResponse<Unit>>
{
    public async Task<ValidatedResponse<Unit>> Handle(AcceptAddAccountRequestCommand command, CancellationToken cancellationToken)
    {

        return ValidatedResponse<Unit>.EmptySuccessResponse();
    }
}
