using FluentValidation;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Permissions.Queries.GetEmployerRelationships;

public class GetEmployerRelationshipsQueryValidator : AbstractValidator<GetEmployerRelationshipsQuery>
{
    public const string ConditionalParamsValidationMessage = "If one of Ukprn or AccountlegalentityPublicHashedId has a value then both properties must be populated.";
    
    public GetEmployerRelationshipsQueryValidator(IAccountReadRepository accountReadRepository)
    {
        RuleFor(a => a.AccountHashedId).ValidateAccount(accountReadRepository);

        When(x => !string.IsNullOrWhiteSpace(x.AccountlegalentityPublicHashedId) || x.Ukprn.HasValue, () =>
        {
            RuleFor(x => x.AccountlegalentityPublicHashedId)
                .NotEmpty()
                .WithMessage(ConditionalParamsValidationMessage);

            RuleFor(x => x.Ukprn)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                    .WithMessage(ConditionalParamsValidationMessage);

            RuleFor(x => x.Ukprn)
                .CheckUkprnFormat();
        });
    }
}