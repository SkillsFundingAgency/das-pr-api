namespace SFA.DAS.PR.Domain.Entities;

public class Request
{
    public Guid Id { get; set; }

    public RequestType RequestType { get; set; }

    public long Ukprn { get; set; }

    public string RequestedBy { get; set; } = null!;

    public DateTime RequestedDate { get; set; }

    public long? AccountLegalEntityId { get; set; }

    public string? EmployerOrganisationName { get; set; }

    public string? EmployerContactFirstName { get; set; }

    public string? EmployerContactLastName { get; set; }

    public string? EmployerContactEmail { get; set; }

    public string? EmployerPAYE { get; set; }

    public string? EmployerAORN { get; set; }

    public RequestStatus Status { get; set; }

    public string? ActionedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public virtual Provider Provider { get; set; } = null!;

    public virtual AccountLegalEntity? AccountLegalEntity { get; set; }

    public virtual List<PermissionRequest> PermissionRequests { get; set; } = [];
}