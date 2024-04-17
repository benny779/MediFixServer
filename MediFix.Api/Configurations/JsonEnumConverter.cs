using System.Text.Json;
using System.Text.Json.Serialization;

namespace MediFix.Api.Configurations;

public record JsonEnum(int Value, string Name);

public class JsonEnumConverter<TEnum> : JsonConverter<TEnum>
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
            case JsonTokenType.StartObject:
                {
                    // TODO: Extract a method
                    int? value = null;
                    string? name = null;

                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndObject)
                        {
                            break;
                        }

                        if (reader.TokenType is not JsonTokenType.PropertyName)
                        {
                            continue;
                        }

                        var propertyName = reader.GetString();
                        reader.Read();

                        switch (propertyName?.ToLower())
                        {
                            case "value" when reader.TokenType is JsonTokenType.Number:
                                value = reader.GetInt32();
                                break;
                            case "name" when reader.TokenType is JsonTokenType.String:
                                name = reader.GetString();
                                break;
                        }
                    }

                    if (value.HasValue && name is not null)
                    {
                        return GetEnumValue(typeToConvert, value.Value);
                    }

                    throw CreateException(typeToConvert.Name);
                }
            case JsonTokenType.String:
                {
                    var stringToConvert = reader.GetString();

                    if (Enum.TryParse(typeToConvert, stringToConvert, ignoreCase: false, out object? resultA))
                    {
                        return (TEnum)resultA;
                    }

                    if (Enum.TryParse(typeToConvert, stringToConvert, ignoreCase: true, out object? resultB))
                    {
                        return (TEnum)resultB;
                    }

                    throw new JsonException($"'{stringToConvert}' is not defined in '{typeToConvert.Name}'.");
                }
            default:
                throw CreateException(typeToConvert.Name);
        }
    }

    private static JsonException CreateException(string typeToConvert)
        => new($"Cannot convert an enum to '{typeToConvert}'.");

    private static TEnum GetEnumValue(Type typeToConvert, int value)
    {
        var enumValue = (TEnum)Enum.ToObject(typeof(TEnum), value);

        if (!Enum.IsDefined(typeToConvert, enumValue))
        {
            throw new JsonException($"'{enumValue}' is not defined in '{typeToConvert.Name}'.");
        }

        return enumValue;
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

