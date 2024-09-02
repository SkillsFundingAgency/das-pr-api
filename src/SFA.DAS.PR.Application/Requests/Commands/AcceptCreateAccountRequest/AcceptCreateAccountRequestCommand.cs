namespace SFA.DAS.PR.Application.Requests.Commands.AcceptCreateAccountRequest;

public sealed class AcceptCreateAccountRequestCommand
{
    public required string ActionedBy { get; set; }
    public required AccountDetails Account { get; set; }
    public required AccountLegalEntityDetails AccountLegalEntity { get; set; }
}

public record AccountDetails(long Id, string Name);
public record AccountLegalEntityDetails(long Id, string Name);
