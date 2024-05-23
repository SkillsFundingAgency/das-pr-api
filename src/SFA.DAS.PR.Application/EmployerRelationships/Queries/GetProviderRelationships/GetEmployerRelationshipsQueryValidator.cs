using FluentValidation;

namespace SFA.DAS.PR.Application.Permissions.Queries.GetEmployerRelationships;

public class GetEmployerRelationshipsQueryValidator : AbstractValidator<GetEmployerRelationshipsQuery>
{
    public const string AccountHashedIdValidationMessage = "An AccountHashedId must be supplied.";
    public GetEmployerRelationshipsQueryValidator()
    {
        RuleFor(a => a.AccountHashedId)
            .NotEmpty()
            .WithMessage(AccountHashedIdValidationMessage);
    }
}