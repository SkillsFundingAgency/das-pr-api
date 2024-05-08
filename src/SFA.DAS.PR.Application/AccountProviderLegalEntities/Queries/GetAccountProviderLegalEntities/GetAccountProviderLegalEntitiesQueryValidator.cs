using FluentValidation;
using SFA.DAS.PR.Application.Validators;

namespace SFA.DAS.PR.Application.AccountProviderLegalEntities.Queries.GetAccountProviderLegalEntities;
public class GetAccountProviderLegalEntitiesQueryValidator : AbstractValidator<GetAccountProviderLegalEntitiesQuery>
{
    public const string UkprnAccountHashIdValidationMessage = "Either one of Ukprn or AccountHashId is required.";
    public GetAccountProviderLegalEntitiesQueryValidator()
    {
        When(x => string.IsNullOrEmpty(x.AccountHashedId), () => RuleFor(x => x.Ukprn)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(UkprnAccountHashIdValidationMessage)
            .CheckUkprnFormat());

        RuleFor(x => x.AccountHashedId)
            .NotEmpty()
            .WithMessage(UkprnAccountHashIdValidationMessage)
            .When(x => !x.Ukprn.HasValue);

        RuleFor(model => model.Operations).ContainsValidOperations();
    }
}