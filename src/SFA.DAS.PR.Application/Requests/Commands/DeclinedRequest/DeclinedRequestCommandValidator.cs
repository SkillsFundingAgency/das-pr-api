using FluentValidation;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Requests.Commands.DeclinedRequest;

public sealed class DeclinedRequestCommandValidator : AbstractValidator<DeclinedRequestCommand>
{
    public const string ActionedByValidationMessage = "The ActionedBy property is required";
    public DeclinedRequestCommandValidator(IRequestReadRepository _requestReadRepository)
    {
        RuleFor(a => a.ActionedBy).NotEmpty().WithMessage(ActionedByValidationMessage);

        RuleFor(a => new RequestIdValidationObject()
        {
            RequestId = a.RequestId,
            RequestStatuses = new[] { RequestStatus.New, RequestStatus.Sent },
            RequestType = null
        })
        .ValidateRequest(_requestReadRepository, RequestsValidator.DeclinedRequestValidationMessage);
    }
}
