using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Application.ProviderRelationships.Queries.GetProviderRelationships;

public class ProviderRelationshipModel
{
    public long? AccountLegalEntityId { get; set; }
    public string? AgreementId { get; set; }
    public Guid? RequestId { get; set; }
    public string EmployerName { get; set; } = null!;
    public bool? HasCreateCohortPermission { get; set; }
    public bool? HasRecruitmentPermission { get; set; }
    public bool? HasRecruitmentWithReviewPermission { get; set; }

    public static implicit operator ProviderRelationshipModel(ProviderRelationship source) =>
        new()
        {
            AccountLegalEntityId = source.AccountLegalEntityId,
            AgreementId = source.AgreementId,
            RequestId = source.RequestId,
            EmployerName = source.EmployerName,
            HasRecruitmentPermission = source.HasRecruitmentPermission,
            HasRecruitmentWithReviewPermission = source.HasRecruitmentWithReviewPermission,
            HasCreateCohortPermission = source.HasCreateCohortPermission
        };
}
