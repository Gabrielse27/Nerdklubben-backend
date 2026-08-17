using Microsoft.Extensions.Configuration;
using Nerdklubben.Application.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Nerdklubben.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendConfirmationEmailAsync(string toEmail, string recipientName)
    {
        var apiKey = _configuration["SendGrid:ApiKey"];
        var fromEmail = _configuration["SendGrid:FromEmail"];
        var fromName = _configuration["SendGrid:FromName"];

        var client = new SendGridClient(apiKey);
        var from = new EmailAddress(fromEmail, fromName);
        var to = new EmailAddress(toEmail, recipientName);

        var subject = "Ansökan mottagen - Nerdklubben";
        var plainTextContent = $"Hej {recipientName}!\n\nTack för din ansökan till Nerdklubben. Vi har tagit emot dina uppgifter och återkommer så snart vi kan.\n\nVänliga hälsningar,\nNerdklubben";
        var htmlContent = $"<h3>Hej {recipientName}!</h3><p>Tack för din ansökan till <strong>Nerdklubben</strong>.</p><p>Vi har tagit emot dina uppgifter och återkommer så snart vi har granskat dem.</p><br><p>Vänliga hälsningar,<br><em>Nerdklubben Team,<br>Gabriel Seres</em></p>";

        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        await client.SendEmailAsync(msg);
    }
}