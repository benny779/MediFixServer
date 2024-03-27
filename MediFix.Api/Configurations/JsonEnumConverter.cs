using System.Text.Json;
using System.Text.Json.Serialization;

namespace MediFix.Api.Configurations;

public record JsonEnum(int Value, string Name);

public class JsonEnumConverter<TEnum> : JsonConverter<TEnum>
    where TEnum : Enum
{
    public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TryGetInt32(out int value))
        {
            return (TEnum)Enum.ToObject(typeof(TEnum), value);
        }

        return (TEnum)Enum.Parse(typeToConvert, reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        var enumType = value.GetType();

        if (Attribute.IsDefined(enumType, typeof(FlagsAttribute)))
        {
            var flagsArray = Enum.GetValues(enumType)
                .Cast<Enum>()
                .Where(value.HasFlag)
                .Select(ParseEnum);

            JsonSerializer.Serialize(writer, flagsArray, options);
        }
        else
        {
            JsonSerializer.Serialize(writer, ParseEnum(value), options);
        }
    }

    private static JsonEnum ParseEnum(object value)
    {
        var enumType = value.GetType();
        var enumValue = Convert.ChangeType(value, Enum.GetUnderlyingType(enumType));
        var enumName = Enum.GetName(enumType, value)!;

        return new JsonEnum(Convert.ToInt32(enumValue), enumName);
    }

    private static JsonEnum ParseEnum(Enum value)
        => ParseEnum((object)value);
}

public class JsonEnumConverterFactory : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsEnum;
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        Type converterType = typeof(JsonEnumConverter<>).MakeGenericType(typeToConvert);
        return (JsonConverter)Activator.CreateInstance(converterType)!;
    }
}



//if (value is null)
//    writer.WriteNullValue();
//else
//    JsonSerializer.Serialize(writer, value.Value, options);

