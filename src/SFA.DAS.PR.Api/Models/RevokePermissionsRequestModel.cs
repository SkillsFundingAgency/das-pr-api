using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.PR.Api.Models;

public class RevokePermissionsRequestModel
{
    public long? Ukprn { get; set; }
    public string AccountLegalEntityPublicHashedId { get; set; }
    public Operation[] OperationsToRevoke { get; set; }

    public RevokePermissionsRequestModel() { }

    public RevokePermissionsRequestModel(long? ukprn, string accountLegalEntityPublicHashedId, Operation[] operationsToRevoke) : this()
    {
        Ukprn = ukprn;
        AccountLegalEntityPublicHashedId = accountLegalEntityPublicHashedId;
        OperationsToRevoke = operationsToRevoke;
    }

}