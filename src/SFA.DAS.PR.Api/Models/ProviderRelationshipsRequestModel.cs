namespace SFA.DAS.PR.Api.Models;

public class ProviderRelationshipsRequestModel
{
    public string? EmployerName { get; set; }
    public bool? HasPendingRequest { get; set; }
    public int? PageSize { get; set; }
    public int? PageNumber { get; set; }
    public bool? HasCreateAdvertWithReviewPermission { get; set; }
    public bool? HasCreateAdvertPermission { get; set; }
    public bool? HasCreateCohortPermission { get; set; }
}
