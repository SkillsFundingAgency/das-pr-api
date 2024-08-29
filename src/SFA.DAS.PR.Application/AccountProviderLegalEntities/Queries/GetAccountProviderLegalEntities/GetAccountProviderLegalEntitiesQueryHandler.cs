using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.PR.Application.AccountProviderLegalEntities.Queries.GetAccountProviderLegalEntities;
public class GetAccountProviderLegalEntitiesQueryHandler(IAccountProviderLegalEntitiesReadRepository _accountProviderLegalEntitiesReadRepository) : IRequestHandler<GetAccountProviderLegalEntitiesQuery, ValidatedResponse<GetAccountProviderLegalEntitiesQueryResult>>
{
    public async Task<ValidatedResponse<GetAccountProviderLegalEntitiesQueryResult>> Handle(GetAccountProviderLegalEntitiesQuery query, CancellationToken cancellationToken)
    {
        if (query.Operations.Exists(o => o == Operation.Recruitment))
        {
            query.Operations.Add(Operation.RecruitmentRequiresReview);
        }

        List<AccountProviderLegalEntity> accountProviderLegalEntities = await _accountProviderLegalEntitiesReadRepository.GetAccountProviderLegalEntities(query.Ukprn, query.AccountHashedId, query.Operations, cancellationToken);

        GetAccountProviderLegalEntitiesQueryResult result = new(accountProviderLegalEntities.Select(a => (AccountProviderLegalEntityModel)a).ToList());

        return new ValidatedResponse<GetAccountProviderLegalEntitiesQueryResult>(result);
    }
}