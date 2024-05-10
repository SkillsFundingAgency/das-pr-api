using FluentValidation;
using SFA.DAS.PR.Application.Common.Validators;

namespace SFA.DAS.PR.Application.Permissions.Queries.HasRelationshipWithPermission;

public class HasRelationshipWithPermissionQueryValidator : AbstractValidator<HasRelationshipWithPermissionQuery>
{
    public const string UkprnValidationMessage = "A Ukprn must be supplied.";

    public HasRelationshipWithPermissionQueryValidator()
    {
        RuleFor(a => a.Ukprn)
            .NotEmpty()
            .WithMessage(UkprnValidationMessage);

        RuleFor(a => a.Ukprn)
            .CheckUkprnFormat();

        RuleFor(a => a.Operation).IsValidOperation();
    }
}