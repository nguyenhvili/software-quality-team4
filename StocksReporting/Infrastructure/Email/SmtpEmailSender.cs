using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using StocksReporting.Application.Report;

namespace StocksReporting.Infrastructure.Email;

public class SmtpEmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;

    public SmtpEmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(string to, string subject, string body, string[] attachments)
    {
        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse(_configuration["EmailSettings:SenderEmail"]));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;

        var builder = new BodyBuilder { TextBody = body };

        foreach (var attachmentPath in attachments)
        {
            builder.Attachments.Add(attachmentPath);
        }

        message.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(_configuration["EmailSettings:SmtpServer"], int.Parse(_configuration["EmailSettings:Port"] ?? "587"), SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(_configuration["EmailSettings:Username"], _configuration["EmailSettings:Password"]);
        await smtp.SendAsync(message);
        await smtp.DisconnectAsync(true);
    }
}
