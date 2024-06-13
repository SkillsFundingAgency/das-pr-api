using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.PR.Domain.Convertors;

[ExcludeFromCodeCoverage]
public class JsonStringEnumMemberConverter<T> : JsonConverter<T> where T : Enum
{
    public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        foreach (var field in typeof(T).GetFields())
        {
            if (field.GetCustomAttribute<EnumMemberAttribute>()?.Value == value)
            {
                return (T)field.GetValue(null);
            }
        }
        return (T)Enum.Parse(typeof(T), value, true);
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        var enumMember = value.GetType()
            .GetField(value.ToString())
            ?.GetCustomAttribute<EnumMemberAttribute>();

        writer.WriteStringValue(enumMember?.Value ?? value.ToString());
    }
}