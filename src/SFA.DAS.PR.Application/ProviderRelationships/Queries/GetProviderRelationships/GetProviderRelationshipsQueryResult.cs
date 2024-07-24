namespace SFA.DAS.PR.Application.ProviderRelationships.Queries.GetProviderRelationships;

public class GetProviderRelationshipsQueryResult
{
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public int TotalCount { get; set; }
    public IEnumerable<ProviderRelationshipModel> Employers { get; set; } = [];
}
