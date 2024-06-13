using System.Runtime.Serialization;

namespace SFA.DAS.PR.Domain.Extensions;

public static class EnumExtensions
{
    public static string ToEnumString<T>(T type)
    {
        Type enumType = typeof(T);
        string name = Enum.GetName(enumType, type!)!;
        EnumMemberAttribute enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name)!.GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
        return enumMemberAttribute.Value!;
    }

    public static T ToEnum<T>(string str)
    {
        var enumType = typeof(T);
        foreach (var name in Enum.GetNames(enumType))
        {
            var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name)!.GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
            if (enumMemberAttribute.Value == str) return (T)Enum.Parse(enumType, name);
        }
        return default!;
    }
}