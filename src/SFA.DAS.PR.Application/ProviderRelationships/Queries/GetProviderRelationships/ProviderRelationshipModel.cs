using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Application.ProviderRelationships.Queries.GetProviderRelationships;

public class ProviderRelationshipModel
{
    public long Ukprn { get; set; }
    public long? AccountLegalEntityId { get; set; }
    public string? AgreementId { get; set; }
    public Guid? RequestId { get; set; }
    public string EmployerName { get; set; } = null!;
    public bool? HasCreateCohortPermission { get; set; }
    public bool? HasCreateAdvertPermission { get; set; }
    public bool? HasCreateAdvertWithReviewPermission { get; set; }

    public static implicit operator ProviderRelationshipModel(ProviderRelationship source) =>
        new()
        {
            Ukprn = source.Ukprn,
            AccountLegalEntityId = source.AccountLegalEntityId,
            AgreementId = source.AgreementId,
            RequestId = source.RequestId,
            EmployerName = source.EmployerName,
            HasCreateAdvertPermission = source.HasCreateAdvertPermission,
            HasCreateAdvertWithReviewPermission = source.HasCreateAdvertWithReviewPermission,
            HasCreateCohortPermission = source.HasCreateCohortPermission
        };
}
