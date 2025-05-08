using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using StocksReportingLibrary.Application.Services.Email;
using StocksReportingLibrary.Configuration;
using Microsoft.Extensions.Logging;

namespace StocksReportingLibrary.Infrastructure.Email;

public class SmtpEmailSender : IEmailSender
{
    private readonly EmailSettings _settings;
    private readonly ILogger<SmtpEmailSender> _logger;

    public SmtpEmailSender(IOptions<EmailSettings> options, ILogger<SmtpEmailSender> logger) 
    {
        _settings = options.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(string to, string subject, string body, string[] attachments)
    {
        _logger.LogInformation("Sending email to: {To}, Subject: {Subject}", to, subject);

        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(_settings.SenderEmail));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;

        var builder = new BodyBuilder { TextBody = body };

        foreach (var attachmentPath in attachments)
        {
            builder.Attachments.Add(attachmentPath);
        }

        message.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        try
        {
            await smtp.ConnectAsync(_settings.SmtpServer, _settings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_settings.Username, _settings.Password);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);

            _logger.LogInformation("Email sent successfully to: {To}", to);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email to: {To}", to);
            throw;
        }
    }
}
