﻿using Microsoft.EntityFrameworkCore;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.PR.Data.Repositories;
public class AccountProviderLegalEntitiesReadRepository(IProviderRelationshipsDataContext _providerRelationshipsDataContext) : IAccountProviderLegalEntitiesReadRepository
{
    public async Task<List<AccountProviderLegalEntity>> GetAccountProviderLegalEntities(long? ukprn, string? accountHashId, List<Operation> operations, CancellationToken cancellationToken)
    {
        var query = _providerRelationshipsDataContext.AccountProviderLegalEntities.AsNoTracking()
            .Include(a => a.AccountProvider)
                .ThenInclude(a => a.Account)
            .Include(a => a.Permissions)
            .Include(a => a.AccountLegalEntity)
            .Where(t =>
                (!ukprn.HasValue || t.AccountProvider.ProviderUkprn == ukprn)
                && (string.IsNullOrWhiteSpace(accountHashId) || t.AccountProvider.Account!.HashedId == accountHashId)
                && (t.Permissions.Any(p => EF.Constant(operations).Contains(p.Operation)))
                && t.AccountLegalEntity.Deleted == null
        );
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<AccountProviderLegalEntity?> GetAccountProviderLegalEntity(long? ukprn, long accountLegalEntityId, CancellationToken cancellationToken)
    {
        return await _providerRelationshipsDataContext.AccountProviderLegalEntities.AsNoTracking()
            .Include(a => a.AccountProvider)
            .ThenInclude(p => p.Provider)
            .Include(a => a.Permissions)
            .Include(a => a.AccountLegalEntity)
        .FirstOrDefaultAsync(a =>
            a.AccountLegalEntityId == accountLegalEntityId &&
            a.AccountProvider.ProviderUkprn == ukprn &&
            a.AccountLegalEntity.Deleted == null,
            cancellationToken
        );
    }

    public async Task<AccountProviderLegalEntity?> GetAccountProviderLegalEntityByProvider(long ukprn, long accountLegalEntityId, CancellationToken cancellationToken)
    {
        return await _providerRelationshipsDataContext.AccountProviderLegalEntities.AsNoTracking()
            .Include(a => a.AccountProvider)
                .ThenInclude(a => a.Provider)
            .Include(a => a.AccountLegalEntity)
            .Include(a => a.Permissions)
        .FirstOrDefaultAsync(a =>
            a.AccountProvider.ProviderUkprn == ukprn &&
            a.AccountLegalEntityId == accountLegalEntityId,
            cancellationToken
        );
    }

    public async Task<bool> AccountProviderLegalEntityExists(long ukprn, long accountLegalEntityId, CancellationToken cancellationToken)
    {
        return await _providerRelationshipsDataContext.AccountProviderLegalEntities.AsNoTracking()
            .Include(a => a.AccountProvider)
            .Include(a => a.Permissions)
            .Include(a => a.AccountLegalEntity)
        .AnyAsync(a =>
            a.AccountLegalEntityId == accountLegalEntityId &&
            a.AccountProvider.ProviderUkprn == ukprn &&
            a.AccountLegalEntity.Deleted == null,
            cancellationToken
        );
    }
}