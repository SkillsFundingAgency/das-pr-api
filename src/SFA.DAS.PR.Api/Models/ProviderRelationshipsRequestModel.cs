namespace SFA.DAS.PR.Api.Models;

public class ProviderRelationshipsRequestModel
{
    public string? EmployerName { get; set; }
    public bool? HasCreateCohortPermission { get; set; }
    public bool? HasRecruitmentPermission { get; set; }
    public bool? HasRecruitmentWithReviewPermission { get; set; }
    public bool? HasPendingRequest { get; set; }
    public int? PageSize { get; set; }
    public int? PageNumber { get; set; }
}
