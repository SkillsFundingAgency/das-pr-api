using FluentValidation;

namespace SFA.DAS.PR.Application.Requests.Queries.GetRequest;

public class GetRequestQueryValidator : AbstractValidator<GetRequestQuery>
{
    public static readonly string RequestIdValidationMessage = "A RequestId must be provided.";
    public GetRequestQueryValidator()
    {
        RuleFor(a => a.RequestId)
            .NotEmpty().WithMessage(RequestIdValidationMessage)
                .Must(ValidateGuid).WithMessage(RequestIdValidationMessage);
    }

    private bool ValidateGuid(string requestId)
    {
        return Guid.TryParse(requestId, out _);
    }
}
