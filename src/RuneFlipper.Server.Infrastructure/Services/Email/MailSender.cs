using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace RuneFlipper.Server.Infrastructure.Services.Email;

public class MailSender(IOptions<MailSenderOptions> options, ILogger<MailSender> logger) : IEmailSender
{
    private readonly ILogger _logger = logger;
    private MailSenderOptions Options { get; } = options.Value;

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        if (string.IsNullOrWhiteSpace(Options.SendGridKey))
        {
            throw new Exception("Null SendGridKey");
        }
        await Execute(Options.SendGridKey, subject, message, toEmail);
    }

    private async Task Execute(string apiKey, string subject, string message, string toEmail)
    {
        var client = new SendGridClient(apiKey);

        var msg = new SendGridMessage
        {
            From = new EmailAddress("emailconfirmation@runeflipper.com", "RuneFlipper"),
            Subject = subject,
            PlainTextContent = message,
            HtmlContent = message
        };

        msg.AddTo(new EmailAddress(toEmail));
        msg.SetClickTracking(false, false);

        var response = await client.SendEmailAsync(msg);

        _logger.LogInformation(response.IsSuccessStatusCode
                               ? $"Email to {toEmail} queued successfully!"
                               : $"Failure Email to {toEmail}");
    }
}
