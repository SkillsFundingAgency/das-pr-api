using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.AccountProviders.Queries.GetAccountProviders;

public class GetAccountProvidersQueryHandler(IAccountProvidersReadRepository accountProvidersReadRepository) : IRequestHandler<GetAccountProvidersQuery, ValidatedResponse<GetAccountProvidersQueryResult>>
{
    private readonly IAccountProvidersReadRepository _accountProvidersReadRepository = accountProvidersReadRepository;

    public async Task<ValidatedResponse<GetAccountProvidersQueryResult>> Handle(GetAccountProvidersQuery query, CancellationToken cancellationToken)
    {
        List<AccountProvider> accountProviders = await _accountProvidersReadRepository.GetAccountProviders(query.AccountId, cancellationToken);

        if (!accountProviders.Any())
        {
            return ValidatedResponse<GetAccountProvidersQueryResult>.EmptySuccessResponse();
        }

        GetAccountProvidersQueryResult result = new(query.AccountId, accountProviders.Select(a => (AccountProviderModel)a).ToList());

        return new ValidatedResponse<GetAccountProvidersQueryResult>(result);
    }
}