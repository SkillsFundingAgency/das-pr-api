﻿using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.PR.Domain.Models;

namespace SFA.DAS.PR.Application.Requests.Queries.LookupRequests;

public class LookupRequestsQuery : IRequest<ValidatedResponse<RequestModel?>>, IUkprnEntity
{
    public long? Ukprn { get; set; }
    public string? Paye { get; set; }
    public string? Email { get; set; }
    public long? AccountLegalEntityId { get; set; }

    public LookupRequestsQuery(long? ukprn, string? paye, string? email, long? accountLegalEntityId)
    {
        Ukprn = ukprn;
        Paye = paye;
        Email = email;
        AccountLegalEntityId = accountLegalEntityId;
    }
}
