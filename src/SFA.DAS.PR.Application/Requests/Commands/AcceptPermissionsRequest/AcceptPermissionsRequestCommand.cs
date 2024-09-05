using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Requests.Commands.AcceptPermissionsRequest;

public sealed class AcceptPermissionsRequestCommand : IRequest<ValidatedResponse<Unit>>, IRequestEntity
{
    public Guid RequestId { get; set; }
    public required string ActionedBy { get; set; }
}
