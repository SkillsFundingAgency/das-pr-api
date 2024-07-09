namespace SFA.DAS.PR.Domain.Entities;

public class ProviderRelationship
{
    public long Ukprn { get; set; }
    public long? AccountLegalEntityId { get; set; }
    public string? AgreementId { get; set; }
    public long? AccountProviderLegalEntityId { get; set; }
    public Guid? RequestId { get; set; }
    public string EmployerName { get; set; } = null!;
    public bool? HasCreateCohortPermission { get; set; }
    public bool? HasCreateAdvertPermission { get; set; }
    public bool? HasCreateAdvertWithReviewPermission { get; set; }
}
