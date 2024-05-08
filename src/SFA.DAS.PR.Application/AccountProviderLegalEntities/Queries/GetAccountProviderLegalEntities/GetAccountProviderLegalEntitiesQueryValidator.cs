using FluentValidation;
using SFA.DAS.PR.Application.Validators;

namespace SFA.DAS.PR.Application.AccountProviderLegalEntities.Queries.GetAccountProviderLegalEntities;
public class GetAccountProviderLegalEntitiesQueryValidator : AbstractValidator<GetAccountProviderLegalEntitiesQuery>
{
    public const string UkprnAccountHashIdValidationMessage = "Either one of Ukprn or AccountHashId is required.";
    public const string OperationFilterValidationMessage = "Currently at least one operation filter must to be supplied.";
    public const string OperationFilterFormatValidationMessage = "Operation filter values are limited to 0, 1 or 2.";
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
