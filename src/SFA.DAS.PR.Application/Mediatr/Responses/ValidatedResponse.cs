using FluentValidation.Results;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PR.Application.Mediatr.Responses
{
    [ExcludeFromCodeCoverage]
    public class ValidatedResponse
    {
    }

    [ExcludeFromCodeCoverage]
    public class ValidatedResponse<TModel> : ValidatedResponse
    {
        public static ValidatedResponse<TModel> EmptySuccessResponse() => new();
        private readonly IList<ValidationFailure> _errorMessages = new List<ValidationFailure>();
        public ValidatedResponse() { }
        public ValidatedResponse(TModel model) => Result = model;
        public ValidatedResponse(IList<ValidationFailure> validationErrors) => _errorMessages = validationErrors;
        public TModel? Result { get; } = default;
        public IReadOnlyCollection<ValidationFailure> Errors => new ReadOnlyCollection<ValidationFailure>(_errorMessages);
        public bool IsValidResponse => !_errorMessages.Any();
    }
}
