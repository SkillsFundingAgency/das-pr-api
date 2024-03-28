using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.AccountProviders.Queries.GetAccountProviders;
public class GetAccountProvidersQueryHandler(IAccountProvidersReadRepository _accountProvidersReadRepository) : IRequestHandler<GetAccountProvidersQuery, ValidatedResponse<GetAccountProvidersQueryResult>>
{
    public async Task<ValidatedResponse<GetAccountProvidersQueryResult>> Handle(GetAccountProvidersQuery query, CancellationToken cancellationToken)
    {
        List<AccountProvider> accountProviders = await _accountProvidersReadRepository.GetAccountProviders(query.AccountHashedId, cancellationToken);

        GetAccountProvidersQueryResult result = new(query.AccountHashedId, accountProviders.Select(a => (AccountProviderModel)a).ToList());

        return new ValidatedResponse<GetAccountProvidersQueryResult>(result);
    }
}