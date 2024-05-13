using FluentValidation.Results;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PR.Application.Mediatr.Responses;

[ExcludeFromCodeCoverage]
public class ValidatedBooleanResult : ValidatedResponse
{
    public static ValidatedBooleanResult EmptySuccessResponse() => new ValidatedBooleanResult();
    private readonly IList<ValidationFailure> _errorMessages = new List<ValidationFailure>();
    public ValidatedBooleanResult() { }
    public ValidatedBooleanResult(bool result) => Result = result;
    public ValidatedBooleanResult(IList<ValidationFailure> validationErrors) => _errorMessages = validationErrors;
    public bool? Result { get; }
    public IReadOnlyCollection<ValidationFailure> Errors => new ReadOnlyCollection<ValidationFailure>(_errorMessages);
    public bool IsValidResponse => !_errorMessages.Any();
}
