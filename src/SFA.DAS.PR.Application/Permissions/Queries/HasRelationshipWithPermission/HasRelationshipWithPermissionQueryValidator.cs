using FluentValidation;
using SFA.DAS.PR.Application.Validators;

namespace SFA.DAS.PR.Application.Permissions.Queries.HasRelationshipWithPermission;

public class HasRelationshipWithPermissionQueryValidator : AbstractValidator<HasRelationshipWithPermissionQuery>
{
    public const string UkprnValidationMessage = "A Ukprn needs to be supplied.";

    public HasRelationshipWithPermissionQueryValidator()
    {
        RuleFor(a => a.Ukprn).Cascade(CascadeMode.Stop).NotEmpty().WithMessage(UkprnValidationMessage).CheckUkprnFormat();
        RuleFor(a => a.Operation).IsValidOperation();
    }
}
