using ErrorOr;
using StocksReportingLibrary.Domain.EmailAttachmentPath;
using StocksReportingLibrary.Domain.EmailMessage;
using StocksReportingLibrary.Domain.EmailMessage.Enums;

namespace Test.Unit.Domain;

public class EmailMessageUnitTest
{
    [Fact]
    public void EmailMessageCreate_ValidInput_CorrectValidation()
    {
        var recipient = "me@gmail.com";
        var subject = "Report";
        var body = "New report sent";
        var attachments = new List<EmailAttachmentPath>();
        attachments.Add(EmailAttachmentPath.Create("2025-04-22_stocks_report.csv"));

        new When()
            .UseValidRecipient(recipient)
            .UseValidSubject(subject)
            .UseValidBody(body)
            .UseValidAttachments(attachments)
            .Create()
            .EmailMessageIsValid();
    }

    [Fact]
    public void EmailMessageCreate_RecipientEmptyString_IncorrectValidation()
    {
        var recipient = "";
        var subject = "Report";
        var body = "New report sent";
        var attachments = new List<EmailAttachmentPath>();
        attachments.Add(EmailAttachmentPath.Create("2025-04-22_stocks_report.csv"));

        new When()
            .UseInvalidRecipient(recipient)
            .UseValidSubject(subject)
            .UseValidBody(body)
            .UseValidAttachments(attachments)
            .Create()
            .EmailMessageIsNotValid();
    }

    [Fact]
    public void EmailMessageCreate_RecipientNullString_IncorrectValidation()
    {
        var subject = "Report";
        var body = "New report sent";
        var attachments = new List<EmailAttachmentPath>();
        attachments.Add(EmailAttachmentPath.Create("2025-04-22_stocks_report.csv"));

        new When()
            .UseValidSubject(subject)
            .UseValidBody(body)
            .UseValidAttachments(attachments)
            .Create()
            .EmailMessageIsNotValid();
    }

    [Fact]
    public void EmailMessageCreate_RecipientMissingAtSign_IncorrectValidation()
    {
        var recipient = "megmail.com";
        var subject = "Report";
        var body = "New report sent";
        var attachments = new List<EmailAttachmentPath>();
        attachments.Add(EmailAttachmentPath.Create("2025-04-22_stocks_report.csv"));

        new When()
            .UseInvalidRecipient(recipient)
            .UseValidSubject(subject)
            .UseValidBody(body)
            .UseValidAttachments(attachments)
            .Create()
            .EmailMessageIsNotValid();
    }

    [Fact]
    public void EmailMessageCreate_RecipientMissingUser_IncorrectValidation()
    {
        var recipient = "@gmail.com";
        var subject = "Report";
        var body = "New report sent";
        var attachments = new List<EmailAttachmentPath>();
        attachments.Add(EmailAttachmentPath.Create("2025-04-22_stocks_report.csv"));

        new When()
            .UseInvalidRecipient(recipient)
            .UseValidSubject(subject)
            .UseValidBody(body)
            .UseValidAttachments(attachments)
            .Create()
            .EmailMessageIsNotValid();
    }

    [Fact]
    public void EmailMessageCreate_SubjectEmptyString_IncorrectValidation()
    {
        var recipient = "me@gmai.com";
        var subject = "";
        var body = "New report sent";
        var attachments = new List<EmailAttachmentPath>();
        attachments.Add(EmailAttachmentPath.Create("2025-04-22_stocks_report.csv"));

        new When()
            .UseValidRecipient(recipient)
            .UseInvalidSubject(subject)
            .UseValidBody(body)
            .UseValidAttachments(attachments)
            .Create()
            .EmailMessageIsNotValid();
    }

    [Fact]
    public void EmailMessageCreate_SubjectNullString_IncorrectValidation()
    {
        var recipient = "me@gmai.com";
        var body = "New report sent";
        var attachments = new List<EmailAttachmentPath>();
        attachments.Add(EmailAttachmentPath.Create("2025-04-22_stocks_report.csv"));

        new When()
            .UseValidRecipient(recipient)
            .UseValidBody(body)
            .UseValidAttachments(attachments)
            .Create()
            .EmailMessageIsNotValid();
    }

    [Fact]
    public void EmailMessageCreate_BodyEmptyString_IncorrectValidation()
    {
        var recipient = "me@gmai.com";
        var subject = "Report";
        var body = "";
        var attachments = new List<EmailAttachmentPath>();
        attachments.Add(EmailAttachmentPath.Create("2025-04-22_stocks_report.csv"));

        new When()
            .UseValidRecipient(recipient)
            .UseValidSubject(subject)
            .UseInvalidBody(body)
            .UseValidAttachments(attachments)
            .Create()
            .EmailMessageIsNotValid();
    }

    [Fact]
    public void EmailMessageCreate_BodyNullString_IncorrectValidation()
    {
        var recipient = "me@gmai.com";
        var subject = "Report";
        var attachments = new List<EmailAttachmentPath>();
        attachments.Add(EmailAttachmentPath.Create("2025-04-22_stocks_report.csv"));

        new When()
            .UseValidRecipient(recipient)
            .UseValidSubject(subject)
            .UseValidAttachments(attachments)
            .Create()
            .EmailMessageIsNotValid();
    }

    [Fact]
    public void EmailMessageCreate_ValidInput_CorrectStatus()
    {
        var recipient = "me@gmail.com";
        var subject = "Report";
        var body = "New report sent";
        var attachments = new List<EmailAttachmentPath>();
        attachments.Add(EmailAttachmentPath.Create("2025-04-22_stocks_report.csv"));

        new When()
            .UseValidRecipient(recipient)
            .UseValidSubject(subject)
            .UseValidBody(body)
            .UseValidAttachments(attachments)
            .Create()
            .EmailMessageIsValid()
            .EmailMessageStatusIsPending();
    }

    [Fact]
    public void EmailMessageRecordSent_ValidInput_CorrectStatus()
    {
        var recipient = "me@gmail.com";
        var subject = "Report";
        var body = "New report sent";
        var attachments = new List<EmailAttachmentPath>();
        attachments.Add(EmailAttachmentPath.Create("2025-04-22_stocks_report.csv"));
        var emailMessage = EmailMessage.Create(recipient, subject, body, attachments);


        new When()
            .UseEmailMessage(emailMessage.Value)
            .RecordSent()
            .EmailMessageStatusIsSent();
    }

    [Fact]
    public void EmailMessageMarkAsInProgress_ValidInput_CorrectStatus()
    {
        var recipient = "me@gmail.com";
        var subject = "Report";
        var body = "New report sent";
        var attachments = new List<EmailAttachmentPath>();
        attachments.Add(EmailAttachmentPath.Create("2025-04-22_stocks_report.csv"));
        var emailMessage = EmailMessage.Create(recipient, subject, body, attachments);


        new When()
            .UseEmailMessage(emailMessage.Value)
            .MarkAsInProgress()
            .EmailMessageStatusIsInProgress();
    }

    [Fact]
    public void EmailMessageRecordRetry_ValidInput_CorrectStatus()
    {
        var recipient = "me@gmail.com";
        var subject = "Report";
        var body = "New report sent";
        var attachments = new List<EmailAttachmentPath>();
        attachments.Add(EmailAttachmentPath.Create("2025-04-22_stocks_report.csv"));
        var emailMessage = EmailMessage.Create(recipient, subject, body, attachments);


        new When()
            .UseEmailMessage(emailMessage.Value)
            .RecordRetry()
            .EmailMessageStatusIsRetryable();
    }

    [Fact]
    public void EmailMessageRecordRetry_ValidInputOutOfRetries_CorrectStatus()
    {
        var recipient = "me@gmail.com";
        var subject = "Report";
        var body = "New report sent";
        var attachments = new List<EmailAttachmentPath>();
        attachments.Add(EmailAttachmentPath.Create("2025-04-22_stocks_report.csv"));
        var emailMessage = EmailMessage.Create(recipient, subject, body, attachments);
        emailMessage.Value.RecordRetry();
        emailMessage.Value.RecordRetry();
        emailMessage.Value.RecordRetry();

        new When()
            .UseEmailMessage(emailMessage.Value)
            .RecordRetry()
            .EmailMessageStatusIsFailed();
    }

    private sealed class When
    {
        private string recipient;
        private string subject;
        private string body;
        private IEnumerable<EmailAttachmentPath> attachments;
        private ErrorOr<EmailMessage> result;
        private EmailMessage message;

        public When()
        {

        }

        public When UseValidRecipient(string input)
        {
            Assert.Null(recipient);
            recipient = input;
            return this;
        }

        public When UseInvalidRecipient(string input)
        {
            Assert.Null(recipient);
            recipient = input;
            return this;
        }

        public When UseValidSubject(string input)
        {
            Assert.Null(subject);
            subject = input;
            return this;
        }

        public When UseInvalidSubject(string input)
        {
            Assert.Null(subject);
            subject = input;
            return this;
        }

        public When UseValidBody(string input)
        {
            Assert.Null(body);
            body = input;
            return this;
        }

        public When UseInvalidBody(string input)
        {
            Assert.Null(body);
            body = input;
            return this;
        }

        public When UseValidAttachments(IEnumerable<EmailAttachmentPath> input)
        {
            Assert.Null(attachments);
            attachments = input;
            return this;
        }

        public When UseEmailMessage(EmailMessage input)
        {
            Assert.Null(message);
            message = input;
            return this;
        }

        public When RecordSent()
        {
            Assert.NotNull(message);
            message.RecordSent();
            return this;
        }
        public When RecordRetry()
        {
            Assert.NotNull(message);
            message.RecordRetry();
            return this;
        }

        public When MarkAsInProgress()
        {
            Assert.NotNull(message);
            message.MarkAsInProgress();
            return this;
        }

        public When Create()
        {
            result = EmailMessage.Create(recipient, subject, body, attachments);
            return this;
        }

        public When EmailMessageIsValid()
        {
            Assert.False(result.IsError);
            Assert.Equal(result.Value.EmailRecipient.Value, recipient);
            Assert.Equal(result.Value.EmailBody.Value, body);
            Assert.Equal(result.Value.EmailSubject.Value, subject);
            Assert.Equal(result.Value.EmailAttachments, attachments);
            return this;
        }

        public When EmailMessageIsNotValid()
        {
            Assert.True(result.IsError);
            return this;
        }

        public When EmailMessageStatusIsPending()
        {
            Assert.Equal(EmailSendStatus.Pending, result.Value.EmailSendStatus);
            return this;
        }

        public When EmailMessageStatusIsSent()
        {
            Assert.Equal(EmailSendStatus.Sent, message.EmailSendStatus);
            return this;
        }

        public When EmailMessageStatusIsInProgress()
        {
            Assert.Equal(EmailSendStatus.InProgress, message.EmailSendStatus);
            return this;
        }

        public When EmailMessageStatusIsFailed()
        {
            Assert.Equal(EmailSendStatus.Failed, message.EmailSendStatus);
            return this;
        }

        public When EmailMessageStatusIsRetryable()
        {
            Assert.Equal(EmailSendStatus.Retryable, message.EmailSendStatus);
            return this;
        }
    }
}
