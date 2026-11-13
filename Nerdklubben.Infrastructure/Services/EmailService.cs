using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using Nerdklubben.Application.Interfaces;


namespace Nerdklubben.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public EmailService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task SendConfirmationEmailAsync(string toEmail, string recipientName)
    {
        var apiKey = _configuration["Brevo:ApiKey"];
        var fromEmail = _configuration["Brevo:FromEmail"];
        var fromName = _configuration["Brevo:FromName"];

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new InvalidOperationException("Brevo API Key saknas i konfigurationen.");
        }

        var payload = new
        {
            sender = new
            {
                name = fromName,
                email = fromEmail
            },
            to = new[]
            {
                new
                {
                    email = toEmail,
                    name = recipientName
                }
            },
            subject = "Ansökan mottagen - Nerdklubben",
            
            htmlContent = $"<h3>Hej {recipientName}!</h3><p>Tack för din ansökan till <strong>Nerdklubben</strong>.</p><p>Vi har tagit emot dina uppgifter och återkommer så snart vi kan.</p><p>Med vänliga hälsningar,<br><strong>Nerdklubben</strong></p>",
            textContent = $"Hej {recipientName}!\n\nTack för din ansökan till Nerdklubben. Vi har tagit emot dina uppgifter och återkommer så snart vi kan.\n\nMed vänliga hälsningar,\nNerdklubben"
        };

        using var request = new HttpRequestMessage(HttpMethod.Post, "https://api.brevo.com/v3/smtp/email");
        request.Headers.Add("api-key", apiKey);
        request.Headers.Add("accept", "application/json");
        request.Content = JsonContent.Create(payload);

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync();
            throw new Exception($"Fel vid utskick av email via Brevo ({response.StatusCode}): {errorBody}");
        }
    }
}