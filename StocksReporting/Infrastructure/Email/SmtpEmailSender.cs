using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using StocksReporting.Application.Report;

namespace StocksReporting.Infrastructure.Email;

public class SmtpEmailSender : IEmailSender
{
    public async Task SendEmailAsync(string to, string subject, string body, string[] attachments)
    {
        var message = new MimeMessage();
        message.From.Add(MailboxAddress.Parse("no-reply@stocksreporting.com"));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;

        var builder = new BodyBuilder { TextBody = body };

        foreach (var attachmentPath in attachments)
        {
            builder.Attachments.Add(attachmentPath);
        }

        message.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        await smtp.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync("pv260reports@gmail.com", "gffd oyas dbhc hkhr");
        await smtp.SendAsync(message);
        await smtp.DisconnectAsync(true);
    }
}
