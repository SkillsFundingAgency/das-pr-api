using Microsoft.EntityFrameworkCore;
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

    public async Task<List<Operation>> GetOperations(long ukprn, string publicHashedId, CancellationToken cancellationToken)
    {
        var operations = await _providerRelationshipsDataContext.Permissions
            .AsNoTracking()
            .Where(x => x.AccountProviderLegalEntity.AccountProvider.ProviderUkprn == ukprn
                        && x.AccountProviderLegalEntity.AccountLegalEntity.PublicHashedId == publicHashedId
                        && x.AccountProviderLegalEntity.AccountLegalEntity.Deleted == null)
            .Select(x => x.Operation)
            .ToListAsync(cancellationToken);

        return operations.ToList();
    }

    public async Task<bool> GetHasPermissions(long ukprn, long accountLegalEntityId, List<Operation> operations, CancellationToken cancellationToken)
    {
        var permissions = await _providerRelationshipsDataContext.Permissions
            .AsNoTracking()
            .Where(x => x.AccountProviderLegalEntity.AccountProvider.ProviderUkprn == ukprn
                        && x.AccountProviderLegalEntity.AccountLegalEntity.Id == accountLegalEntityId
                        && x.AccountProviderLegalEntity.AccountLegalEntity.Deleted == null)
        .ToListAsync(cancellationToken);

        return permissions.Count > 0 && operations.TrueForAll(operation => permissions.Exists(x => x.Operation == operation));
    }
}
