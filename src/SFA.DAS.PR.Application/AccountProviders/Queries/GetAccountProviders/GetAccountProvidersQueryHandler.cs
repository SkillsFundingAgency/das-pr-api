﻿using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.AccountProviders.Queries.GetAccountProviders;
public class GetAccountProvidersQueryHandler(IAccountLegalEntityReadRepository accountLegalEntityReadRepository) : IRequestHandler<GetAccountProvidersQuery, ValidatedResponse<GetAccountProvidersQueryResult>>
{
    public async Task<ValidatedResponse<GetAccountProvidersQueryResult>> Handle(GetAccountProvidersQuery query, CancellationToken cancellationToken)
    {
        List<AccountLegalEntity> legalEntities = await accountLegalEntityReadRepository.GetAccountLegalEntities(query.AccountId, cancellationToken);

        GetAccountProvidersQueryResult result = new(query.AccountId, AccountProviderModel.BuildAccountProviderModels(legalEntities));

        return new ValidatedResponse<GetAccountProvidersQueryResult>(result);
    }
}