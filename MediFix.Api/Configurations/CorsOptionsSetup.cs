using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace MediFix.Api.Configurations;

internal class CorsOptionsSetup(IConfiguration configuration) : IConfigureOptions<CorsOptions>
{
    public const string DefaultPolicyName = "CorsPolicy";

    public void Configure(CorsOptions options)
    {
        var corsConfiguration = configuration
            .GetRequiredSection(CorsConfiguration.SectionName)
            .Get<CorsConfiguration>()!;

        options.AddPolicy(DefaultPolicyName, policyBuilder =>
        {
            policyBuilder
                .WithOrigins(corsConfiguration.AllowedOrigins)
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    }
}

internal class CorsConfiguration
{
    public const string SectionName = "Cors";
    public string[] AllowedOrigins { get; init; } = [];
}