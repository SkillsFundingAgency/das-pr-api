﻿using Microsoft.EntityFrameworkCore;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Data.Repositories;

public class PermissionsReadRepository(IProviderRelationshipsDataContext _providerRelationshipsDataContext) : IPermissionsReadRepository
{
    public async Task<bool> HasPermissionWithRelationship(long ukprn, Operation operation, CancellationToken cancellationToken) => 
        await _providerRelationshipsDataContext.AccountProviderLegalEntities
                    .Include(a => a.AccountProvider)
                    .Include(a => a.Permissions)
                .AnyAsync(p => 
                    p.AccountProvider.ProviderUkprn == ukprn && 
                    p.Permissions.Any(a => a.Operation == operation) &&
                    p.AccountLegalEntity.Deleted == null, 
            cancellationToken
        );
}