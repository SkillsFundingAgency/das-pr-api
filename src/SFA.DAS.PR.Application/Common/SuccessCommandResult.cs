using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PR.Application.Common
{
    [ExcludeFromCodeCoverage]
    public record SuccessCommandResult(bool IsSuccess = true);
}
