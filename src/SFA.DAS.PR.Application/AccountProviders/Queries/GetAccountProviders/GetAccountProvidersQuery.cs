using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;

namespace SFA.DAS.PR.Application.AccountProviders.Queries.GetAccountProviders;

public class GetAccountProvidersQuery(string accountHashedId) : IRequest<ValidatedResponse<GetAccountProvidersQueryResult>>
{
    public string AccountHashedId { get; } = accountHashedId;
}
