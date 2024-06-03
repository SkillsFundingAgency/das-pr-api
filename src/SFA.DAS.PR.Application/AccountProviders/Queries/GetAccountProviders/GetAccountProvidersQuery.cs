using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;

namespace SFA.DAS.PR.Application.AccountProviders.Queries.GetAccountProviders;

public class GetAccountProvidersQuery(long accountId) : IRequest<ValidatedResponse<GetAccountProvidersQueryResult>>
{
    public long AccountId { get; } = accountId;
}
