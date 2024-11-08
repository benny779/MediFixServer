﻿namespace MediFix.Api.Configurations;

internal static class OptionsConfiguration
{
    public static IServiceCollection AddCustomConfigureOptions(this IServiceCollection services)
    {
        services.ConfigureOptions<JwtOptionsSetup>();
        services.ConfigureOptions<JwtBearerOptionsSetup>();

        services.ConfigureOptions<SwaggerGenOptionsSetup>();
        
        services.ConfigureOptions<CorsOptionsSetup>();

        services.ConfigureOptions<EmailOptionsSetup>();

        return services;
    }
}
