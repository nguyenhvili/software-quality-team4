using System.Net;
using System.Net.Mail;
using StocksReporting.Application.Common.Interfaces;

namespace StocksReporting.Infrastructure.Email;

public class SmtpEmailSender : IEmailSender
{
    public async Task SendEmailAsync(string to, string subject, string body, string[] attachments)
    {
        using var message = new MailMessage("no-reply@stocksreporting.com", to, subject, body);
        foreach (var attachment in attachments)
        {
            message.Attachments.Add(new Attachment(attachment));
        }

        using var smtpClient = new SmtpClient("smtp.example.com")
        {
            Port = 587,
            Credentials = new NetworkCredential("yourusername", "yourpassword"),
            EnableSsl = true
        };

        await smtpClient.SendMailAsync(message);
    }
}
