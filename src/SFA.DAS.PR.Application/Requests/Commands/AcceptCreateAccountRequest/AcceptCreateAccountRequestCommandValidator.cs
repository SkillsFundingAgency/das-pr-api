﻿using FluentValidation;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Requests.Commands.AcceptCreateAccountRequest;

public sealed class AcceptCreateAccountRequestCommandValidator : AbstractValidator<AcceptCreateAccountRequestCommand>
{
    public AcceptCreateAccountRequestCommandValidator(IRequestReadRepository requestReadRepository)
    {
        RuleFor(a => new RequestIdValidationObject()
            { 
                RequestId = a.RequestId, 
                RequestStatuses = new[] { RequestStatus.New, RequestStatus.Sent },
                RequestType = RequestType.CreateAccount
            }
        )
        .ValidateRequest(requestReadRepository, RequestsValidator.CreateAccountRequestValidationMessage);
    }
}