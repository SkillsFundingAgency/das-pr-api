using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Requests.Commands.DeclinedRequest;

public class DeclinedRequestCommand : IRequest<ValidatedResponse<Unit>>, IRequestEntity
{
    public required Guid RequestId { get; set; }
    public string? ActionedBy { get; set; }
}