using FluentValidation;
using SFA.DAS.PR.Domain.Entities;
using SFA.DAS.PR.Domain.Interfaces;

namespace SFA.DAS.PR.Application.Common.Validators;
public static class OperationsValidator
{
    public const string OperationFilterValidationMessage = "An operation filter must to be supplied.";
    public const string OperationsFilterValidationMessage = "Currently at least one operation filter must to be supplied.";
    public const string OperationFilterFormatValidationMessage = "Operation filter values are limited to 0, 1 or 2.";

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
}