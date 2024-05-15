using FluentValidation;

namespace SFA.DAS.PR.Application.Permissions.Queries.GetPermissions;

public class GetPermissionsQueryValidator : AbstractValidator<GetPermissionsQuery>
{
    public const string AccountHashedIdValidationMessage = "An AccountHashedId must be supplied.";
    public GetPermissionsQueryValidator()
    {
        RuleFor(a => a.AccountHashedId)
            .NotEmpty()
            .WithMessage(AccountHashedIdValidationMessage);
    }
}