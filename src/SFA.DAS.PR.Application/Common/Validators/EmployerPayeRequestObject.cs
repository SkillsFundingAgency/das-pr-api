using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Common.Validators;

public class EmployerPayeRequestObject : IUkprnEntity
{
    public long? Ukprn { get; set; }
    public RequestStatus[] RequestStatuses { get; set; } = [];
    public string? EmployerPAYE { get; set; }
}
