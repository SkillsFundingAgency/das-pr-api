using FluentValidation;

namespace SFA.DAS.PR.Application.Permissions.Queries.GetAllPermissionsForAccount;

public class GetAllPermissionsForAccountQueryValidator : AbstractValidator<GetAllPermissionsForAccountQuery>
{
    public const string AccountHashedIdValidationMessage = "An AccountHashedId must be supplied.";
    public GetAllPermissionsForAccountQueryValidator()
    {
        RuleFor(a => a.AccountHashedId)
            .NotEmpty()
            .WithMessage(AccountHashedIdValidationMessage);
    }
}