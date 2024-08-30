namespace SFA.DAS.PR.Api.Models;

public class ProviderRelationshipsRequestModel
{
    public string? SearchTerm { get; set; }
    public bool? HasCreateCohortPermission { get; set; }
    public required bool HasRecruitmentPermission { get; set; }
    public required bool HasRecruitmentWithReviewPermission { get; set; }
    public required bool HasNoRecruitmentPermission { get; set; }
    public bool? HasPendingRequest { get; set; }
    public int? PageSize { get; set; }
    public int? PageNumber { get; set; }
}
