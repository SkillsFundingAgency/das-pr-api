using FluentValidation;

namespace SFA.DAS.PR.Application.AccountProviderLegalEntities.Queries.GetAccountProviderLegalEntities;
public class GetAccountProviderLegalEntitiesQueryValidator : AbstractValidator<GetAccountProviderLegalEntitiesQuery>
{
    public const string UkprnAccountHashIdValidationMessage = "Either one of Ukprn or AccountHashId is required.";
    public const string UkprnFormatValidationMessage = "Currently a Ukprn must start with the value 1 and should be 8 digits long.";
    public const string OperationFilterValidationMessage = "Currently at least one operation filter must to be supplied.";
    public const string OperationFilterFormatValidationMessage = "Operation filter values are limited to 0, 1 or 2.";
    public GetAccountProviderLegalEntitiesQueryValidator()
    {
        RuleFor(x => x.Ukprn)
            .Must(Ukprn => Ukprn.HasValue)
            .WithMessage(UkprnAccountHashIdValidationMessage)
            .When(x => string.IsNullOrEmpty(x.AccountHashedId));

        RuleFor(x => x.AccountHashedId)
            .Must(AccountHashedId => !string.IsNullOrWhiteSpace(AccountHashedId))
            .WithMessage(UkprnAccountHashIdValidationMessage)
            .When(x => !x.Ukprn.HasValue);

        RuleFor(model => model.Ukprn)
            .Must(ukprn => ukprn.ToString()!.StartsWith('1') && ukprn?.ToString().Length == 8)
            .WithMessage(UkprnFormatValidationMessage)
            .When(model => model.Ukprn.HasValue);

        RuleFor(model => model.Operations)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .When(model => model.Operations == null || !model.Operations.Any())
            .WithMessage(OperationFilterValidationMessage)
            .Must(operations =>  (operations.Any() && operations.All(op => (int)op == 0 || (int)op == 1 || (int)op == 2)))
            .WithMessage(OperationFilterFormatValidationMessage);
    }
}
