using FluentValidation;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Requests.Commands.CreateNewAccountRequest;

public class CreateNewAccountRequestCommandValidator : AbstractValidator<CreateNewAccountRequestCommand>
{
    public static readonly string UkprnValidationMessage = "A Ukprn must be provided.";
    public static readonly string EmployerContactEmailValidationMessage = "EmployerContactEmail must be in the correct email format.";

    public CreateNewAccountRequestCommandValidator(
        IRequestReadRepository requestReadRepository,
        IProviderReadRepository providerReadRepository
    )
    {
        RuleFor(x => x.Ukprn)
            .NotEmpty()
            .WithMessage(UkprnValidationMessage);

        RuleFor(x => x.Ukprn)
            .IsValidUkprn(providerReadRepository);

        RuleFor(a => new EmployerPayeRequestObject() { Ukprn = a.Ukprn, EmployerPAYE = a.EmployerPAYE, RequestStatuses = new[] { RequestStatus.New, RequestStatus.Sent } })
            .ValidateRequest(requestReadRepository);

        RuleFor(a => a.Operations).ValidateOperationCombinations();

        RuleFor(a => a.EmployerContactEmail)
            .Matches(ValidationRegex.EmailRegex)
            .WithMessage(EmployerContactEmailValidationMessage)
            .When(a => !string.IsNullOrWhiteSpace(a.EmployerContactEmail));
    }
}
