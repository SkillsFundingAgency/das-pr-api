using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.AccountProviders.Queries.GetAccountProviders;
public class GetAccountProvidersQueryHandler(IAccountProvidersReadRepository _accountProvidersReadRepository) : IRequestHandler<GetAccountProvidersQuery, ValidatedResponse<GetAccountProvidersQueryResult>>
{
    public async Task<ValidatedResponse<GetAccountProvidersQueryResult>> Handle(GetAccountProvidersQuery query, CancellationToken cancellationToken)
    {
        List<AccountLegalEntity> legalEntities = await _accountProvidersReadRepository.GetAccountProviders(query.AccountId, cancellationToken);

        List<Account> accounts = legalEntities.Select(a => a.Account).ToList();

        List<AccountProvider> providers = accounts.Select(a => a.AccountProviders).ToList();

        GetAccountProvidersQueryResult result = new(query.AccountId, accountProviders.Select(a => (AccountProviderModel)a).ToList());

        return new ValidatedResponse<GetAccountProvidersQueryResult>(result);
    }
}