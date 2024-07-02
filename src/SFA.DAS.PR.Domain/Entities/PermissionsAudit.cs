using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PR.Domain.Entities;

[ExcludeFromCodeCoverage]
public class PermissionsAudit
{
    public Guid Id { get; set; }
    public required DateTime Eventtime { get; set; }
    public required string Action { get; set; }
    public required long Ukprn {  get; set; }
    public required long AccountLegalEntityId { get; set; }
    public Guid? EmployerUserRef { get; set; }
    public required string Operations { get; set; }
}
