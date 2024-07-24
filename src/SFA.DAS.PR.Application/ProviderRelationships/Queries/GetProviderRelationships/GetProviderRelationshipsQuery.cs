using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.ProviderRelationships.Queries.GetProviderRelationships;

public class GetProviderRelationshipsQuery : IUkprnEntity, IRequest<ValidatedResponse<GetProviderRelationshipsQueryResult>>
{
    public long? Ukprn { get; set; }
    public string? SearchTerm { get; set; }
    public bool? HasPendingRequest { get; set; }
    public int PageSize { get; set; }
    public int PageNumber { get; set; }
    public bool? HasCreateCohortPermission { get; set; }
    public bool? HasRecruitWithReviewPermission { get; set; }
    public bool? HasRecruitPermission { get; set; }
    public bool? HasNoRecruitPermissions { get; set; }
}
