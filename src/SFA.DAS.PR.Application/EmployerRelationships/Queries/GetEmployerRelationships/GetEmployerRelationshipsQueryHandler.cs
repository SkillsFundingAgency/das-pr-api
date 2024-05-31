using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Permissions.Queries.GetEmployerRelationships;

public class GetEmployerRelationshipsQueryHandler(IEmployerRelationshipsReadRepository employerRelationshipsReadRepository) : IRequestHandler<GetEmployerRelationshipsQuery, ValidatedResponse<GetEmployerRelationshipsQueryResult>>
{
    public async Task<ValidatedResponse<GetEmployerRelationshipsQueryResult>> Handle(GetEmployerRelationshipsQuery query, CancellationToken cancellationToken)
    {
        Account? account = await employerRelationshipsReadRepository.GetRelationships(query.AccountHashedId, query.Ukprn, query.AccountlegalentityPublicHashedId, cancellationToken);

        GetEmployerRelationshipsQueryResult queryResult = new GetEmployerRelationshipsQueryResult(account?.AccountLegalEntities.Select(a => (AccountLegalEntityPermissionsModel)a).ToList());

        return new ValidatedResponse<GetEmployerRelationshipsQueryResult>(queryResult);
    }
}