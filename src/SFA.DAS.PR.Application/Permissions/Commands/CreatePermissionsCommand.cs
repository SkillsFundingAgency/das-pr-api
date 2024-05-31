using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Application.Permissions.Commands;

public class CreatePermissionsCommand
{
    public required string UserRef { get; set; }

    public required long Ukprn { get; set; }

    public required long AccountLegalEntityId { get; set; }

    public required Operation[] Operations { get; set; }
}
