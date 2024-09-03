using SFA.DAS.PR.Application.Requests.Commands.AcceptCreateAccountRequest;

namespace SFA.DAS.PR.Api.Models;

public class AcceptCreateAccountRequestModel
{
    public required string ActionedBy { get; set; }
    public required AccountDetails AccountDetails { get; set; }
    public required AccountLegalEntityDetails AccountLegalEntityDetails { get; set; }
}
