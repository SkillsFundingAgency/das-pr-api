using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.PR.Domain.Models;

public class RequestModel
{
    public Guid RequestId { get; set; }
    public required string RequestType { get; set; }
    public long Ukprn { get; set; }
    public required string ProviderName { get; set; }
    public required string RequestedBy { get; set; }
    public DateTime RequestedDate { get; set; }
    public long? AccountLegalEntityId { get; set; }
    public string? EmployerOrganisationName { get; set; }
    public string? EmployerContactFirstName { get; set; }
    public string? EmployerContactLastName { get; set; }
    public string? EmployerContactEmail { get; set; }
    public string? EmployerPAYE { get; set; }
    public string? EmployerAORN { get; set; }
    public required string Status { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public Operation[] Operations { get; set; } = [];

    public static implicit operator RequestModel(Request source)
    {
        return new RequestModel()
        {
            RequestId = source.Id,
            RequestType = source.RequestType,
            Ukprn = source.Ukprn,
            ProviderName = source.Provider.Name,
            RequestedBy = source.RequestedBy,
            RequestedDate = source.RequestedDate,
            AccountLegalEntityId = source.AccountLegalEntityId,
            EmployerOrganisationName = source.EmployerOrganisationName,
            EmployerContactFirstName = source.EmployerContactFirstName,
            EmployerContactLastName = source.EmployerContactLastName,
            EmployerContactEmail = source.EmployerContactEmail,
            EmployerPAYE = source.EmployerPAYE,
            EmployerAORN = source.EmployerAORN,
            Status = source.Status,
            UpdatedDate = source.UpdatedDate,
            Operations = source.PermissionRequests.Select(a => (Operation)a.Operation).ToArray()
        };
    }
}
