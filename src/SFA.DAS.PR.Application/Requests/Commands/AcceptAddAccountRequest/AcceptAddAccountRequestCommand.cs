using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Requests.Commands.AcceptAddAccountRequest;

public sealed class AcceptAddAccountRequestCommand : IRequest<ValidatedResponse<Unit>>, IRequestEntity
{
    public required Guid RequestId { get; set; }
    public required string ActionedBy { get; set; }
}
