using SFA.DAS.PR.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PR.Application.EmployerRelationships.Queries.GetProviderEmployerRelationship;

[ExcludeFromCodeCoverage]
public class GetProviderEmployerRelationshipQueryResult
{
    public long AccountLegalEntityId { get; set; }

    public string AccountLegalEntitypublicHashedId { get; set; } = null!;

    public string AccountLegalEntityName { get; set; } = null!;

    public long AccountId { get; set; }

    public long Ukprn { get; set; }

    public string ProviderName { get; set; } = null!;

    public Operation[] Operations { get; set; } = [];

    public PermissionAction? LastAction { get; set; }

    public DateTime? LastActionTime {  get; set; }

    public string? LastRequestType { get; set; }

    public DateTime? LastRequestTime {  get; set; }

    public RequestStatus? LastRequestStatus {  get; set; }

    public static implicit operator GetProviderEmployerRelationshipQueryResult(AccountProviderLegalEntity source) => new()
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