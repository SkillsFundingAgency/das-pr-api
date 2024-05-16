using FluentValidation;

namespace SFA.DAS.PR.Application.AccountProviders.Queries.GetAccountProviders;

public class GetAccountProvidersQueryValidator : AbstractValidator<GetAccountProvidersQuery>
{
    public const string AccountProvidersIdValidationMessage = "Account ID needs to be set.";
    public GetAccountProvidersQueryValidator()
    {
        RuleFor(model => model.AccountId).Must(id => id > 0).WithMessage(AccountProvidersIdValidationMessage);
    }
}
