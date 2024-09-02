using FluentValidation;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Requests.Commands.AcceptCreateAccountRequest;

public class AcceptCreateAccountRequestCommandValidator : AbstractValidator<AcceptCreateAccountRequestCommandWrapper>
{
    public AcceptCreateAccountRequestCommandValidator(
        IRequestReadRepository requestReadRepository,
        IProviderReadRepository providerReadRepository,
        IAccountLegalEntityReadRepository accountLegalEntityReadRepository
    )
    {
        RuleFor(a => new RequestIdValidationObject() 
            { 
                RequestId = a.RequestId, 
                RequestStatuses = new[] { RequestStatus.New, RequestStatus.Sent },
                RequestType = RequestType.CreateAccount
            }
        )
        .ValidateRequest(requestReadRepository);
    }
}
