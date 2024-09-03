using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Requests.Commands.AcceptCreateAccountRequest;

public class AcceptCreateAccountRequestCommandWrapper : IRequest<ValidatedResponse<Unit>>, IRequestEntity
{
    public required Guid RequestId { get; set; }
    public required AcceptCreateAccountRequestCommand Command {  get; set; }
}
