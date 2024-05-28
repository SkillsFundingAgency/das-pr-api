using FluentValidation;
using SFA.DAS.PR.Application.Common.Validators;
namespace SFA.DAS.PR.Application.Permissions.Queries.GetEmployerRelationships;

public class GetEmployerRelationshipsQueryValidator : AbstractValidator<GetEmployerRelationshipsQuery>
{
    public const string AccountHashedIdValidationMessage = "An AccountHashedId must be supplied.";
    public const string ConditionalParamsValidationMessage = "If one of Ukprn or AccountlegalentityPublicHashedId has a value then both properties must be populated.";
    
    public GetEmployerRelationshipsQueryValidator()
    {
        RuleFor(a => a.AccountHashedId)
            .NotEmpty()
            .WithMessage(AccountHashedIdValidationMessage);

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