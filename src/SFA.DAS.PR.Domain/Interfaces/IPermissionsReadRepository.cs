﻿using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.PR.Domain.Interfaces;

public interface IPermissionsReadRepository
{
    Task<bool> HasPermissionWithRelationship(long ukprn, Operation operation, CancellationToken cancellationToken);
    Task<List<Operation>> GetOperations(long ukprn, long accountLegalEntityId, CancellationToken cancellationToken);
}
