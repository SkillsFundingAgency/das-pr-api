using Microsoft.EntityFrameworkCore;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.ProviderRelationships.Types.Models;

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

    public async Task<List<Operation>> GetOperations(long ukprn, long accountLegalEntityId, CancellationToken cancellationToken)
    {
        return await _providerRelationshipsDataContext.Permissions
            .AsNoTracking()
            .Where(x => x.AccountProviderLegalEntity.AccountProvider.ProviderUkprn == ukprn
                        && x.AccountProviderLegalEntity.AccountLegalEntity.Id == accountLegalEntityId
                        && x.AccountProviderLegalEntity.AccountLegalEntity.Deleted == null)
            .Select(x => x.Operation)
        .ToListAsync(cancellationToken);
    }
}
