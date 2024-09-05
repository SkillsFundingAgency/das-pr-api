using FluentValidation;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Requests.Commands.AcceptPermissionsRequest;

public sealed class AcceptPermissionsRequestCommandValidator : AbstractValidator<AcceptPermissionsRequestCommand>
{
    public AcceptPermissionsRequestCommandValidator(IRequestReadRepository requestReadRepository)
    {
        RuleFor(a => new RequestIdValidationObject()
            {
                RequestId = a.RequestId,
                RequestStatuses = new[] { RequestStatus.New, RequestStatus.Sent },
                RequestType = RequestType.Permission
            }
        )
        .ValidateRequest(requestReadRepository, RequestsValidator.AcceptPermissionsRequestValidationMessage);
    }
}
