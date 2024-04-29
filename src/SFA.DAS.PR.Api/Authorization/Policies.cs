using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PR.Api.Authorization;

[ExcludeFromCodeCoverage]
public static class Policies
{
    public const string Integration = nameof(Integration);
    public const string Management = nameof(Management);
}