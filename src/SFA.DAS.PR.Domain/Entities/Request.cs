namespace SFA.DAS.PR.Domain.Entities;

public class Request
{
    public Guid Id { get; set; }

    public string RequestType { get; set; } = null!;

    public long Ukprn { get; set; }

    public string ProviderUserFullName { get; set; } = null!;

    public string RequestedBy { get; set; } = null!;

    public DateTime RequestedDate { get; set; }

    public long? AccountLegalEntityId { get; set; }

    public Guid? EmployerUserRef {  get; set; }

    public string? EmployerOrganisationName { get; set; } = null!;

    public string? EmployerContactFirstName { get; set; } = null!;

    public string? EmployerContactLastName { get; set; } = null!;

    public string? EmployerContactEmail { get; set; }

    public string? EmployerPAYE {  get; set; }

    public string? EmployerAORN {  get; set; }

    public string? Status {  get; set; } = null!;

    public DateTime? UpdatedDate { get; set; }

    public virtual Provider Provider { get; set; } = null!;

    public virtual AccountLegalEntity AccountLegalEntity { get; set; } = null!;
}