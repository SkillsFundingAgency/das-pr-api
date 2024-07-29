using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace SFA.DAS.PR.Data.ValueConvertors;

public class EnumConvertor<T> : ValueConverter<T, string>
{
    public EnumConvertor() : base(v => v!.ToString()!, v => (T)Enum.Parse(typeof(T), v)) {}
}