using FluentValidation;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Requests.Commands.CreateAddAccountRequest;

public class CreateAddAccountRequestCommandValidator : AbstractValidator<CreateAddAccountRequestCommand>
{
    public static readonly string UkprnValidationMessage = "A Ukprn must be provided.";
    public static readonly string EmployerContactEmailValidationMessage = "EmployerContactEmail must be in the correct email format.";

    public CreateAddAccountRequestCommandValidator(
        IRequestReadRepository requestReadRepository, 
        IProviderReadRepository providerReadRepository, 
        IAccountLegalEntityReadRepository accountLegalEntityReadRepository
    )
    {
        RuleFor(x => x.Ukprn)
            .NotEmpty()
            .WithMessage(UkprnValidationMessage);

        RuleFor(x => x.Ukprn)
            .IsValidUkprn(providerReadRepository);

        RuleFor(a => a.AccountLegalEntityId)
            .ValidateAccountLegalEntityExists(accountLegalEntityReadRepository);

        RuleFor(a => new RequestValidationObject() { Ukprn = a.Ukprn, AccountLegalEntityId = a.AccountLegalEntityId, RequestStatuses = new[] { RequestStatus.New, RequestStatus.Sent } })
            .ValidateRequest(requestReadRepository);

        RuleFor(a => a.Operations).ValidateOperationCombinations();

        RuleFor(a => a.EmployerContactEmail)
            .EmailAddress()
            .WithMessage(EmployerContactEmailValidationMessage)
            .When(a => !string.IsNullOrWhiteSpace(a.EmployerContactEmail));
    }
}
