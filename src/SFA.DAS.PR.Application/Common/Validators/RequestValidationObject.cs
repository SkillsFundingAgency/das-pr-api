using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Common.Validators;

public class RequestValidationObject : IUkprnEntity, IAccountLegalEntityIdEntity
{
    public long? Ukprn { get; set; }

    public long AccountLegalEntityId { get; set; }
}
