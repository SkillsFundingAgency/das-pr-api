using FluentValidation;
using SFA.DAS.PR.Application.Common.Validators;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Permissions.Queries.HasRelationshipWithPermission;

public class HasRelationshipWithPermissionQueryValidator : AbstractValidator<HasRelationshipWithPermissionQuery>
{
    public const string UkprnValidationMessage = "A Ukprn must be supplied.";

    public HasRelationshipWithPermissionQueryValidator(IProviderReadRepository providerReadRepository)
    {
        RuleFor(x => x.Ukprn)
            .NotEmpty()
            .WithMessage(UkprnValidationMessage);

        RuleFor(a => a.Ukprn)
            .IsValidUkprn(providerReadRepository);

        RuleFor(a => a.Operation).IsValidOperation();
    }
}