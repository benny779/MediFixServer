using MediFix.Domain.Core.Primitives;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MediFix.Api.Configurations;

internal static class JsonConfiguration
{
    public static void SetJsonOptions(JsonOptions options)
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.AllowTrailingCommas = true;

        options.JsonSerializerOptions.Converters.Add(new StronglyTypedIdJsonConverterFactory());
        options.JsonSerializerOptions.Converters.Add(new JsonEnumConverterFactory(compareValueAndName: false));
    }
}