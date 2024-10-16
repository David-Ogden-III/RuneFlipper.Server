namespace MailService;

public class MailSenderOptions
{
    public string? SendGridKey { get; set; } = Environment.GetEnvironmentVariable("SENDGRID_API_KEY");
}
