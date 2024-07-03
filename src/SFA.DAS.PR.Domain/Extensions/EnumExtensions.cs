namespace SFA.DAS.PR.Domain.Extensions;

public static class EnumExtensions
{
    public static T ToEnum<T>(string str) where T : Enum
    {
        if (Enum.TryParse(typeof(T), str, true, out var result))
        {
            return (T)result;
        }
        throw new ArgumentException($"Unable to convert '{str}' to enum {typeof(T).Name}");
    }
}