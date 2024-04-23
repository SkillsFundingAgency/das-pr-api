﻿using SFA.DAS.PR.Application.AccountProviders.Queries.GetAccountProviders;
using Swashbuckle.AspNetCore.Filters;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PR.Api.SwaggerExamples;

[ExcludeFromCodeCoverage]
public class GetAccountProvidersQueryResultExample : IExamplesProvider<GetAccountProvidersQueryResult>
{
    public GetAccountProvidersQueryResult GetExamples()
    {
        GetAccountProvidersQueryResult result = new(8123, new List<AccountProviderModel>()
        {
            new AccountProviderModel()
            {
                Id = 116,
                ProviderUkprn = 10000055,
                ProviderName = "ABINGDON AND WITNEY COLLEGE",
                AccountLegalEntities = new List<AccountLegalEntityModel>()
                {
                    new AccountLegalEntityModel()
                    {
                        Id = 34270,
                        Name = "ADAM & ADAM PROPERTIES LTD",
                        Operations = []
                    },
                    new AccountLegalEntityModel()
                    {
                        Id = 22503,
                        Name = "BAWDEN FUND",
                        Operations = []
                    }
                }
            },
            new AccountProviderModel()
            {
                Id = 562,
                ProviderUkprn = 10000143,
                ProviderName = "BARKING & DAGENHAM LONDON BOROUGH COUNCIL",
                AccountLegalEntities = new List<AccountLegalEntityModel>()
                {
                    new AccountLegalEntityModel()
                    {
                        Id = 34270,
                        Name = "ADAM & ADAM PROPERTIES LTD",
                        Operations = []
                    },
                    new AccountLegalEntityModel()
                    {
                        Id = 22503,
                        Name = "BAWDEN FUND",
                        Operations = []
                    }
                }
            }
        });

        return result;
    }
}
