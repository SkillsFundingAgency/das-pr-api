﻿using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.PR.Application.Permissions.Queries.GetHasPermissions;

public class GetHasPermissionsQuery : IRequest<ValidatedResponse<bool>>, IUkprnEntity, IOperationsEntity
{
    public long? Ukprn { get; set; }
    public long? AccountLegalEntityId { get; set; }
    public List<Operation> Operations { get; set; } = new List<Operation>();
}