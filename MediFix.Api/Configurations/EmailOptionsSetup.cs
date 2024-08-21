using MediFix.Infrastructure.Services.Email;
using Microsoft.Extensions.Options;

namespace MediFix.Api.Configurations;

internal class EmailOptionsSetup : IConfigureOptions<EmailOptions>
{
    private readonly IConfiguration _configuration;

    public EmailOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(EmailOptions options)
    {
        _configuration.GetSection(EmailOptions.SectionName).Bind(options);

        if (options.Username is null ^ options.Password is null)
        {
            throw new ArgumentException("Both Username and Password must be provided together.");
        }
    }
}