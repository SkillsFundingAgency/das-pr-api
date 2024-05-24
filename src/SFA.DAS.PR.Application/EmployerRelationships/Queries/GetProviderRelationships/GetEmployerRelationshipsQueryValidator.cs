using FluentValidation;
using SFA.DAS.PR.Application.Common.Validators;
namespace SFA.DAS.PR.Application.Permissions.Queries.GetEmployerRelationships;

public class GetEmployerRelationshipsQueryValidator : AbstractValidator<GetEmployerRelationshipsQuery>
{
    public const string AccountHashedIdValidationMessage = "An AccountHashedId must be supplied.";
    public GetEmployerRelationshipsQueryValidator()
    {
        RuleFor(a => a.AccountHashedId)
            .NotEmpty()
            .WithMessage(AccountHashedIdValidationMessage);

        //RuleFor(a => a.Ukprn)
        //    .NotEmpty()
        //    .When(a => a.Ukprn.HasValue)
        //    .CheckUkprnFormat();
    }
}