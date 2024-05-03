using Microsoft.EntityFrameworkCore;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Data.Repositories;

public class PermissionsReadRespository(IProviderRelationshipsDataContext providerRelationshipsDataContext) : IPermissionsReadRespository
{
    private readonly IProviderRelationshipsDataContext _providerRelationshipsDataContext = providerRelationshipsDataContext;

    public async Task<bool> GetPermissionsHas(long ukprn, string publicHashedId, List<Operation> operations, CancellationToken cancellationToken)
    {
        var providers =
            await _providerRelationshipsDataContext.AccountProviders.FirstOrDefaultAsync(x => x.ProviderUkprn == ukprn, cancellationToken: cancellationToken);

        if (providers == null) return false;

        var permissions = await _providerRelationshipsDataContext.Permissions
            .AsNoTracking()
            .Include(p => p.AccountProviderLegalEntity)
            .ThenInclude(a => a.AccountProvider)
            .Include(x => x.AccountProviderLegalEntity)
            .ThenInclude(y => y.AccountLegalEntity)
            .Where(x => x.AccountProviderLegalEntity.AccountProvider.ProviderUkprn == ukprn
                && x.AccountProviderLegalEntity.AccountLegalEntity.PublicHashedId == publicHashedId)
            .ToListAsync(cancellationToken);

        return permissions.Any() && operations.TrueForAll(operation => permissions.Exists(x => x.Operation == operation));
    }
}