using FluentValidation;
using SFA.DAS.PR.Application.Common.Validators;

namespace SFA.DAS.PR.Application.EmployerRelationships.Queries.GetProviderEmployerRelationship;

public class GetProviderEmployerRelationshipQueryValidator : AbstractValidator<GetProviderEmployerRelationshipQuery>
{
    public const string UkprnValidationMessage = "A Ukprn must be supplied.";
    public const string AccountLegalEntityIdValidationMessage = "A AccountLegalEntityId must be supplied.";
    public GetProviderEmployerRelationshipQueryValidator()
    {
        RuleFor(a => a.Ukprn)
            .NotEmpty()
            .WithMessage(UkprnValidationMessage)
            .CheckUkprnFormat();

        RuleFor(a => a.AccountLegalEntityId)
            .NotEmpty()
            .WithMessage(AccountLegalEntityIdValidationMessage);
    }
}