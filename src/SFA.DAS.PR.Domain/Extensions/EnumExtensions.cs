namespace SFA.DAS.PR.Domain.Extensions;

public static class EnumExtensions
{
    public static string ToLowerString(this Enum enumValue)
    {
        return enumValue.ToString().ToLower();
    }
}
