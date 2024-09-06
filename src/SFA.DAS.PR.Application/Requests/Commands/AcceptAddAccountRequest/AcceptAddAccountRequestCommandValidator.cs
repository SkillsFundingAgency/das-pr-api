using FluentValidation;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.PR.Data.Repositories;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Requests.Commands.AcceptAddAccountRequest;

public sealed class AcceptAddAccountRequestCommandValidator : AbstractValidator<AcceptAddAccountRequestCommand>
{
    public AcceptAddAccountRequestCommandValidator(IRequestReadRepository requestReadRepository)
    {
        RuleFor(a => new RequestIdValidationObject()
        {
            RequestId = a.RequestId,
            RequestStatuses = new[] { RequestStatus.New, RequestStatus.Sent },
            RequestType = RequestType.AddAccount
        }
        )
        .ValidateRequest(requestReadRepository, RequestsValidator.AcceptAddAccountRequestValidationMessage);
    }
}
