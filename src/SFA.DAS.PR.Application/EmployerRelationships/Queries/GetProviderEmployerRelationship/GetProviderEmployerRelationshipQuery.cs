using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.EmployerRelationships.Queries.GetProviderEmployerRelationship;

public class GetProviderEmployerRelationshipQuery(long? ukprn, long? accountLegalEntityId) : IRequest<ValidatedResponse<GetProviderEmployerRelationshipQueryResult>>, IUkprnEntity
{
    public long? Ukprn { get; set; } = ukprn;
    public long? AccountLegalEntityId { get; set; } = accountLegalEntityId;
}