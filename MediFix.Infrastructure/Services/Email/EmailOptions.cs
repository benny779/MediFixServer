namespace MediFix.Infrastructure.Services.Email;

public class EmailOptions
{
    public const string SectionName = "Email";

    public string SmtpServer { get; init; } = null!;
    public int SmtpPort { get; init; }
    public bool EnableSsl { get; init; }
    public string? Username { get; init; }
    public string? Password { get; init; }
    public string SenderEmail { get; init; } = null!;
    public string SenderName { get; init; } = null!;

    public bool UseAuth => Username is not null && Password is not null;
}