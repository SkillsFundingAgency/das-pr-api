using FluentValidation;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.PR.Application.Common.Validators;
public static class OperationsValidator
{
    public const string OperationFilterValidationMessage = "An operation filter must to be supplied.";
    public const string OperationsFilterValidationMessage = "Currently at least one operation must to be supplied.";
    public const string OperationFilterFormatValidationMessage = "Operation values are limited to 0, 1 or 2.";
    public const string OperationsCombinationValidationMessage = "Operations filter can have a maximum of two items and must be one of the valid combinations: 0, 1, 2, 0,1, 0,2.";
    public static IRuleBuilderOptions<T, List<Operation>> ContainsValidOperations<T>(this IRuleBuilderInitial<T, List<Operation>> ruleBuilder) where T : IOperationsEntity
    {
        return ruleBuilder
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(OperationsFilterValidationMessage)
            .Must(operations => operations.TrueForAll(op =>
            {
                return Enum.TryParse(op.ToString(), out Operation operation) &&
                       Enum.IsDefined(typeof(Operation), operation);
            }))
            .WithMessage(OperationFilterFormatValidationMessage);
    }

    public static IRuleBuilderOptions<T, Operation?> IsValidOperation<T>(this IRuleBuilderInitial<T, Operation?> ruleBuilder) where T : IOperationEntity
    {
        return ruleBuilder
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage(OperationFilterValidationMessage)
            .Must(op => Enum.TryParse(op!.Value.ToString(), out Operation operation) && Enum.IsDefined(typeof(Operation), operation))
            .WithMessage(OperationFilterFormatValidationMessage);
    }

    public static IRuleBuilder<T, List<Operation>> ValidateOperationCombinations<T>(this IRuleBuilderInitial<T, List<Operation>> ruleBuilder) where T : IOperationsEntity
    {
        return ruleBuilder
             .Cascade(CascadeMode.Stop)
             .NotEmpty()
             .WithMessage(OperationsFilterValidationMessage)
             .Must(a => ContainValidCombinations(a))
             .WithMessage(OperationsCombinationValidationMessage);
    }

    private static bool ContainValidCombinations(List<Operation> operations)
    {
        var validCombinations = new List<Operation[]>
        {
            new Operation[] { Operation.CreateCohort },
            new Operation[] { Operation.Recruitment },
            new Operation[] { Operation.RecruitmentRequiresReview },
            new Operation[] { Operation.CreateCohort, Operation.Recruitment },
            new Operation[] { Operation.CreateCohort, Operation.RecruitmentRequiresReview }
        };

        return validCombinations.Any(vc => vc.SequenceEqual(operations.OrderBy(a => a)));
    }
}