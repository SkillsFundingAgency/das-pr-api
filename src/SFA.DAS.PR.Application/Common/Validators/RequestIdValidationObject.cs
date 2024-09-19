using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Common.Validators;

public class RequestIdValidationObject : IRequestEntity
{
    public Guid RequestId { get; set; }
    public RequestStatus[] RequestStatuses { get; set; } = [];
    public RequestType? RequestType { get; set; }
}
