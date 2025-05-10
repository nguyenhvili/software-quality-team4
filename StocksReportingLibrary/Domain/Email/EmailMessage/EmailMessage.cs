using System.ComponentModel.DataAnnotations;
using ErrorOr;
using StocksReportingLibrary.Domain.Common;
using StocksReportingLibrary.Domain.Email.EmailMessage.Enums;
using StocksReportingLibrary.Domain.Email.EmailMessage.ValueObjects;

namespace StocksReportingLibrary.Domain.Email.EmailMessage;

public class EmailMessage : AggregateRoot<EmailMessageId>
{
    public EmailRecipient EmailRecipient { get; private set; }
    public EmailSubject EmailSubject { get; private set; }
    public EmailBody EmailBody { get; private set; }
    private List<EmailAttachmentPath.EmailAttachmentPath> _emailAttachments = new();
    public IReadOnlyList<EmailAttachmentPath.EmailAttachmentPath> EmailAttachments => _emailAttachments.AsReadOnly();
    public EmailSendStatus EmailSendStatus { get; private set; }
    public RetryCount RetryCount { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    private const int MaxRetryCount = 3;

    public EmailMessage()
    {

    }

    private EmailMessage(EmailMessageId emailId, EmailRecipient recipient, EmailSubject subject, EmailBody body, IEnumerable<EmailAttachmentPath.EmailAttachmentPath> attachments, EmailSendStatus status, RetryCount retryCount) : base(emailId)
    {
        EmailRecipient = recipient;
        EmailSubject = subject;
        EmailBody = body;
        _emailAttachments = attachments.ToList();
        EmailSendStatus = status;
        RetryCount = retryCount;
    }

    public void RecordSent()
    {
        EmailSendStatus.UpdateStatus(EEmailSendStatus.Sent);
    }

    public void RecordRetry()
    {
        if (RetryCount.Value >= MaxRetryCount)
        {
            EmailSendStatus.UpdateStatus(EEmailSendStatus.Failed);
            return;
        }

        RetryCount.Increment();
        EmailSendStatus.UpdateStatus(EEmailSendStatus.Retryable);
    }

    public void MarkAsInProgress()
    {
        EmailSendStatus.UpdateStatus(EEmailSendStatus.InProgress);
    }

    public static ErrorOr<EmailMessage> Create(string recipient, string subject, string body, IEnumerable<EmailAttachmentPath.EmailAttachmentPath> attachments)
    {
        if (recipient == null || recipient.Length == 0)
        {
            return Error.Validation("The recipient is empty!");
        }
        var emailValidation = new EmailAddressAttribute();
        if (!emailValidation.IsValid(recipient))
        {
            return Error.Validation("The email is not in valid format!");
        }

        if (subject == null || subject.Length == 0)
        {
            return Error.Validation("The subject is empty!");
        }

        if (body == null || body.Length == 0)
        {
            return Error.Validation("The body is empty!");
        }

        if (attachments.Any(p => p.Path == null || p.Path.Length == 0))
        {
            return Error.Validation("The attachment path is empty!");
        }

        return new EmailMessage(EmailMessageId.CreateUnique(), EmailRecipient.Create(recipient), EmailSubject.Create(subject), EmailBody.Create(body), attachments, EmailSendStatus.CreateDefault(), RetryCount.Create(0));
    }
}