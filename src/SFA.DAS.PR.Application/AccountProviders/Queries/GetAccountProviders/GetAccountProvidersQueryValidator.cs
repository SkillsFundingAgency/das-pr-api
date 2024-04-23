using FluentValidation;

namespace SFA.DAS.PR.Application.AccountProviders.Queries.GetAccountProviders
{
    public class GetAccountProvidersQueryValidator : AbstractValidator<GetAccountProvidersQuery>
    {
        public GetAccountProvidersQueryValidator()
        {
            RuleFor(model => model.AccountId).Must(id => id != 0).WithMessage("Account ID needs to be set.");
        }
    }
}
