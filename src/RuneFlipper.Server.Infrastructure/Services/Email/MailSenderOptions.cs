namespace RuneFlipper.Server.Infrastructure.Services.Email;

public class MailSenderOptions
{
    public string? SendGridKey { get; init; } = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
}
