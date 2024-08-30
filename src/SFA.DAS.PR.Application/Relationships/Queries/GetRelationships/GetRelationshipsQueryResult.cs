using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.PR.Application.Relationships.Queries.GetRelationships;

public class GetRelationshipsQueryResult
{
    public long AccountLegalEntityId { get; set; }

    public string AccountLegalEntitypublicHashedId { get; set; } = null!;

    public string AccountLegalEntityName { get; set; } = null!;

    public long AccountId { get; set; }

    public long Ukprn { get; set; }

    public string ProviderName { get; set; } = null!;

    public Operation[] Operations { get; set; } = [];

    public string? LastAction { get; set; }

    public DateTime? LastActionTime { get; set; }

    public string? LastRequestType { get; set; }

    public DateTime? LastRequestTime { get; set; }

    public string? LastRequestStatus { get; set; }

    public Operation[]? LastRequestOperations { get; set; }

    public static implicit operator GetRelationshipsQueryResult(AccountProviderLegalEntity source) => new()
    {
        AccountLegalEntityId = source.AccountLegalEntityId,
        AccountLegalEntitypublicHashedId = source.AccountLegalEntity.PublicHashedId,
        AccountLegalEntityName = source.AccountLegalEntity.Name,
        AccountId = source.AccountLegalEntity.AccountId,
        Ukprn = source.AccountProvider.ProviderUkprn,
        ProviderName = source.AccountProvider.Provider.Name,
        Operations = source.Permissions.Select(a => a.Operation).ToArray()
    };
}