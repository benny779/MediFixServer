using System.Text.Json;
using System.Text.Json.Serialization;

namespace MediFix.Api.Configurations;

public record JsonEnum(int Value, string Name);

internal class JsonEnumConverter<TEnum>(bool compareValueAndName) : JsonConverter<TEnum>
    where TEnum : Enum
{
    public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        JsonTokenType token = reader.TokenType;

        switch (token)
        {
            case JsonTokenType.Number when reader.TryGetInt32(out int value):
                {
                    return GetEnumValue(typeToConvert, value);
                }
            case JsonTokenType.String:
                {
                    var stringToConvert = reader.GetString();

                    return GetEnumValue(typeToConvert, stringToConvert);
                }
            case JsonTokenType.StartObject:
                {
                    return GetEnumValue(typeToConvert, ref reader, options);
                }
            default:
                throw ExceptionCannotConvert(typeToConvert.Name);
        }
    }

    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
    {
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

    private static TEnum GetEnumValue(Type typeToConvert, int value)
    {
        var enumValue = (TEnum)Enum.ToObject(typeof(TEnum), value);

        if (Enum.IsDefined(typeToConvert, enumValue))
        {
            return enumValue;
        }

        throw ExceptionNotDefined(typeToConvert.Name, $"{value}");
    }

    private static TEnum GetEnumValue(Type typeToConvert, string? name)
    {
        if (Enum.TryParse(typeToConvert, name, ignoreCase: false, out object? resultA))
        {
            return (TEnum)resultA;
        }

        if (Enum.TryParse(typeToConvert, name, ignoreCase: true, out object? resultB))
        {
            return (TEnum)resultB;
        }

        throw ExceptionNotDefined(typeToConvert.Name, name);
    }

    private TEnum GetEnumValue(Type typeToConvert, ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        try
        {
            JsonEnum? jsonEnum = JsonSerializer.Deserialize<JsonEnum>(ref reader, options);

            if (jsonEnum is not null)
            {
                return GetEnumValue(typeToConvert, jsonEnum);
            }
        }
        catch (Exception ex)
        {
            throw ExceptionCannotConvert(typeToConvert.Name, ex);
        }

        throw ExceptionCannotConvert(typeToConvert.Name);
    }

    private TEnum GetEnumValue(Type typeToConvert, JsonEnum jsonEnum)
    {
        var enumValueFromValue = GetEnumValue(typeToConvert, jsonEnum.Value);

        if (!compareValueAndName)
        {
            return enumValueFromValue;
        }

        var enumValueFromName = GetEnumValue(typeToConvert, jsonEnum.Name);

        if (enumValueFromValue.Equals(enumValueFromName))
        {
            return enumValueFromValue;
        }

        throw ExceptionCannotConvert(typeToConvert.Name);
    }

    private static JsonException ExceptionCannotConvert(string typeToConvert, Exception? innerException = null)
        => new($"Error converting JSON to enum '{typeToConvert}'.", innerException);

    private static JsonException ExceptionNotDefined(string typeToConvert, string? name)
        => new($"'{name}' is not defined in '{typeToConvert}'.");
}

public class JsonEnumConverterFactory(bool compareValueAndName = true) : JsonConverterFactory
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsEnum;
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        Type converterType = typeof(JsonEnumConverter<>).MakeGenericType(typeToConvert);

        return (JsonConverter)Activator.CreateInstance(converterType, [compareValueAndName])!;
    }
}