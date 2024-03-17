﻿using MediFix.SharedKernel.Results;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MediFix.Domain.Core.Primitives;


[TypeConverter(typeof(StronglyTypedIdConverter))]
public abstract record StronglyTypedId<TValue>(TValue Value)
    where TValue : notnull
{
    public override string ToString() => Value.ToString()!;

    public static implicit operator TValue(StronglyTypedId<TValue> value) => value.Value;
}

public static class StronglyTypedIdHelper
{
    private static readonly ConcurrentDictionary<Type, Delegate> StronglyTypedIdFactories = new();

    public static Func<TValue, object> GetFactory<TValue>(Type stronglyTypedIdType)
        where TValue : notnull
    {
        return (Func<TValue, object>)StronglyTypedIdFactories
            .GetOrAdd(
                stronglyTypedIdType,
                CreateFactory<TValue>);
    }

    private static Func<TValue, object> CreateFactory<TValue>(Type stronglyTypedIdType)
        where TValue : notnull
    {
        if (!IsStronglyTypedId(stronglyTypedIdType))
            throw new ArgumentException($"Type '{stronglyTypedIdType}' is not a strongly-typed id type", nameof(stronglyTypedIdType));

        var createMethod = stronglyTypedIdType.GetMethod("Create", [typeof(TValue)]);
        var ctor = stronglyTypedIdType.GetConstructor([typeof(TValue)]);

        if (createMethod is null && createMethod?.ReturnType != typeof(Result<TValue>))
            throw new ArgumentException($"Type '{stronglyTypedIdType}' doesn't have a 'Create' method or a constructor with one parameter of type '{typeof(TValue)}'", nameof(stronglyTypedIdType));

        var param = Expression.Parameter(typeof(TValue), "value");
        Expression body = ctor is not null ? Expression.New(ctor, param) : Expression.Call(null, createMethod, param);
        var lambda = Expression.Lambda<Func<TValue, object>>(body, param);
        return lambda.Compile();
    }

    public static bool IsStronglyTypedId(Type type) => IsStronglyTypedId(type, out _);

    public static bool IsStronglyTypedId(Type type, [NotNullWhen(true)] out Type? idType)
    {
        ArgumentNullException.ThrowIfNull(type);

        if (type.BaseType is { } baseType &&
            baseType.GetTypeInfo().IsGenericType &&
            baseType.GetGenericTypeDefinition() == typeof(StronglyTypedId<>))
        {
            idType = baseType.GetGenericArguments()[0];
            return true;
        }

        idType = null;
        return false;
    }
}

public class StronglyTypedIdConverter<TValue>(Type type) : TypeConverter
    where TValue : notnull
{
    private static readonly TypeConverter IdValueConverter = GetIdValueConverter();

    private static TypeConverter GetIdValueConverter()
    {
        var converter = TypeDescriptor.GetConverter(typeof(TValue));
        if (!converter.CanConvertFrom(typeof(string)))
            throw new InvalidOperationException(
                $"Type '{typeof(TValue)}' doesn't have a converter that can convert from string");
        return converter;
    }

    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType)
    {
        return sourceType == typeof(string)
            || sourceType == typeof(TValue)
            || base.CanConvertFrom(context, sourceType);
    }

    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType)
    {
        return destinationType == typeof(string)
            || destinationType == typeof(TValue)
            || base.CanConvertTo(context, destinationType);
    }

    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object? value)
    {
        if (value is string s)
        {
            value = IdValueConverter.ConvertFrom(s);
        }

        if (value is TValue idValue)
        {
            var factory = StronglyTypedIdHelper.GetFactory<TValue>(type);
            return factory(idValue);
        }

        return base.ConvertFrom(context, culture, value ?? new object());
    }

    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        ArgumentNullException.ThrowIfNull(value);

        var stronglyTypedId = (StronglyTypedId<TValue>)value;
        TValue idValue = stronglyTypedId.Value;

        if (destinationType == typeof(string))
            return idValue.ToString()!;

        if (destinationType == typeof(TValue))
            return idValue;

        return base.ConvertTo(context, culture, value, destinationType);
    }
}

public class StronglyTypedIdConverter(Type stronglyTypedIdType) : TypeConverter
{
    private static readonly ConcurrentDictionary<Type, TypeConverter> ActualConverters = new();

    private readonly TypeConverter _innerConverter = ActualConverters.GetOrAdd(stronglyTypedIdType, CreateActualConverter);

    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) =>
        _innerConverter.CanConvertFrom(context, sourceType);
    public override bool CanConvertTo(ITypeDescriptorContext? context, Type? destinationType) =>
        _innerConverter.CanConvertTo(context, destinationType);
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value) =>
        _innerConverter.ConvertFrom(context, culture, value);
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType) =>
        _innerConverter.ConvertTo(context, culture, value, destinationType);


    private static TypeConverter CreateActualConverter(Type stronglyTypedIdType)
    {
        if (!StronglyTypedIdHelper.IsStronglyTypedId(stronglyTypedIdType, out var idType))
            throw new InvalidOperationException($"The type '{stronglyTypedIdType}' is not a strongly typed id");

        var actualConverterType = typeof(StronglyTypedIdConverter<>).MakeGenericType(idType);
        return (TypeConverter)Activator.CreateInstance(actualConverterType, stronglyTypedIdType)!;
    }
}

public class StronglyTypedIdJsonConverter<TStronglyTypedId, TValue> : JsonConverter<TStronglyTypedId>
    where TStronglyTypedId : StronglyTypedId<TValue>
    where TValue : notnull
{
    public override TStronglyTypedId? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is JsonTokenType.Null)
            return null;

        var value = JsonSerializer.Deserialize<TValue>(ref reader, options);
        var factory = StronglyTypedIdHelper.GetFactory<TValue>(typeToConvert);
        return (TStronglyTypedId)factory(value!);
    }

    public override void Write(Utf8JsonWriter writer, TStronglyTypedId? value, JsonSerializerOptions options)
    {
        if (value is null)
            writer.WriteNullValue();
        else
            JsonSerializer.Serialize(writer, value.Value, options);
    }
}

public class StronglyTypedIdJsonConverterFactory : JsonConverterFactory
{
    private static readonly ConcurrentDictionary<Type, JsonConverter> Cache = new();

    public override bool CanConvert(Type typeToConvert)
    {
        return StronglyTypedIdHelper.IsStronglyTypedId(typeToConvert);
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return Cache.GetOrAdd(typeToConvert, CreateConverter);
    }

    private static JsonConverter CreateConverter(Type typeToConvert)
    {
        if (!StronglyTypedIdHelper.IsStronglyTypedId(typeToConvert, out var valueType))
            throw new InvalidOperationException($"Cannot create converter for '{typeToConvert}'");

        var type = typeof(StronglyTypedIdJsonConverter<,>).MakeGenericType(typeToConvert, valueType);
        return (JsonConverter)Activator.CreateInstance(type)!;
    }
}

