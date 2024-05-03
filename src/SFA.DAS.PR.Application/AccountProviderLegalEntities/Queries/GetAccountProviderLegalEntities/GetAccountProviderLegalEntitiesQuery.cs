using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Application.AccountProviderLegalEntities.Queries.GetAccountProviderLegalEntities;
public class GetAccountProviderLegalEntitiesQuery : IRequest<ValidatedResponse<GetAccountProviderLegalEntitiesQueryResult>>
{
    public string? AccountHashedId { get; set; }
    public long? Ukprn { get; set; }
    public List<Operation> Operations { get; set; } = [];
}