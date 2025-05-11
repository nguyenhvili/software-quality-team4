using ErrorOr;

namespace StocksReportingLibrary.Application.Services.Email;

public interface IEmailQueue
{
    public Task<ErrorOr<IEnumerable<EmailQueueItem>>> GetPendingEmailsAsync(int maxCount = 50);
    public Task RecordRetryAsync(Guid emailId);
    public Task MarkAsSentAsync(Guid emailId);
    public Task EnqueueEmailAsync(
        string recipient,
        string subject,
        string body,
        IEnumerable<string> attachmentPaths
    );
    public record EmailQueueItem(
        Guid Id,
        string Recipient,
        string Subject,
        string Body,
        IEnumerable<string> AttachmentPaths,
        DateTime CreatedAt
    );
}
