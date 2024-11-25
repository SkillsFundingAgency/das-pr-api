using FluentValidation;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Permissions.Queries.GetEmployerRelationships;

public class GetEmployerRelationshipsQueryValidator : AbstractValidator<GetEmployerRelationshipsQuery>
{
    public const string AccountHashedIdValidationMessage = "An AccountId must be supplied.";
    public const string AccountIdDoesNotExistValidationMessage = "The provided AccountId does not exist.";
    public GetEmployerRelationshipsQueryValidator(IEmployerRelationshipsReadRepository employerReadRepository, IAccountReadRepository accountReadRepository)
    {
        RuleFor(a => a.AccountId)
            .NotEmpty()
            .WithMessage(AccountHashedIdValidationMessage);

        RuleFor(a => a.AccountId)
            .MustAsync(accountReadRepository.AccountIdExists)
            .WithMessage(AccountIdDoesNotExistValidationMessage);
    }
}