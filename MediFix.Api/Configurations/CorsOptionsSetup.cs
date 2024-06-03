using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Options;

namespace MediFix.Api.Configurations;

internal class CorsOptionsSetup : IConfigureOptions<CorsOptions>
{
    public const string DefaultPolicyName = "CorsPolicy";

    public void Configure(CorsOptions options)
    {
        options.AddPolicy(DefaultPolicyName, policyBuilder =>
        {
            policyBuilder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    }
}
