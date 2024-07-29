namespace SFA.DAS.PR.Domain.Models;

public class ProviderRelationshipsQueryFilters
{
    public long? Ukprn { get; set; }
    public string? SearchTerm { get; set; }
    public bool? HasPendingRequest { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public bool? HasRecruitmentWithReviewPermission { get; set; }
    public bool? HasRecruitmentPermission { get; set; }
    public bool? HasCreateCohortPermission { get; set; }
}
