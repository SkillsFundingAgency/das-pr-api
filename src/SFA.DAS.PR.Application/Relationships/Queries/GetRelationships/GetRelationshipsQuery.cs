using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Relationships.Queries.GetRelationships;

public class GetRelationshipsQuery(long? ukprn, long? accountLegalEntityId) : IRequest<ValidatedResponse<GetRelationshipsQueryResult?>>, IUkprnEntity, IAccountLegalEntityNullableIdEntity
{
    public long? Ukprn { get; set; } = ukprn;
    public long? AccountLegalEntityId { get; set; } = accountLegalEntityId;
}