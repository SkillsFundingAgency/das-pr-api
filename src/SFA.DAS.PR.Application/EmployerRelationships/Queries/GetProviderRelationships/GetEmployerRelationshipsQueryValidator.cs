using FluentValidation;

namespace SFA.DAS.PR.Application.Permissions.Queries.GetEmployerRelationships;

public class GetEmployerRelationshipsQueryValidator : AbstractValidator<GetEmployerRelationshipsQuery>
{
    public const string AccountHashedIdValidationMessage = "An AccountId must be supplied.";
    public GetEmployerRelationshipsQueryValidator()
    {
        RuleFor(a => a.AccountId)
            .NotEmpty()
            .WithMessage(AccountHashedIdValidationMessage);
    }
}