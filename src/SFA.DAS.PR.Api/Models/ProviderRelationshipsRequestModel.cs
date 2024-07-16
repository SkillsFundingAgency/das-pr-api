namespace SFA.DAS.PR.Api.Models;

public class ProviderRelationshipsRequestModel
{
    public string? EmployerName { get; set; }
    public bool? HasPendingRequest { get; set; }
    public int? PageSize { get; set; }
    public int? PageNumber { get; set; }
    public bool? HasCreateCohortPermission { get; set; }
    public bool? HasRecruitWithReviewPermission { get; set; }
    public bool? HasRecruitPermission { get; set; }
    public bool? HasNoRecruitPermissions { get; set; }
}
