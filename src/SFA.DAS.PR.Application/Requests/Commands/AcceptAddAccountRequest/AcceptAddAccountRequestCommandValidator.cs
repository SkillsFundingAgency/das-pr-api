using FluentValidation;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Requests.Commands.AcceptAddAccountRequest;

public sealed class AcceptAddAccountRequestCommandValidator : AbstractValidator<AcceptAddAccountRequestCommand>
{
    public const string ActionedByValidationMessage = "The ActionedBy property is required";
    public AcceptAddAccountRequestCommandValidator(IRequestReadRepository requestReadRepository)
    {
        RuleFor(a => a.ActionedBy).NotEmpty().WithMessage(ActionedByValidationMessage);

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
