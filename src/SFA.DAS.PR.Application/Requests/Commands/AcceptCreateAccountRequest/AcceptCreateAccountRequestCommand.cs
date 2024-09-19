using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Requests.Commands.AcceptCreateAccountRequest;

public sealed class AcceptCreateAccountRequestCommand : IRequest<ValidatedResponse<Unit>>, IRequestEntity
{
    public Guid RequestId { get; set; }
    public string? ActionedBy { get; set; }
    public required AccountDetails Account { get; set; }
    public required AccountLegalEntityDetails AccountLegalEntity { get; set; }
}

public record AccountDetails(long Id, string Name);
public record AccountLegalEntityDetails(long Id, string Name);
