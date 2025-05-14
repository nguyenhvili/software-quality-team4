using ErrorOr;
using Microsoft.Extensions.Logging;
using StocksReportingLibrary.Application.Services.Email;
using StocksReportingLibrary.Application.Services.Persistence;
using StocksReportingLibrary.Domain.EmailMessage.Enums;
using StocksReportingLibrary.Domain.EmailAttachmentPath;

namespace StocksReportingLibrary.Infrastructure.Email;

public class EmailQueueService(
    IQueryObject<Domain.EmailMessage.EmailMessage> emailMessageQuery,
    ILogger<EmailQueueService> logger,
    IRepository<Domain.EmailMessage.EmailMessage> emailMessageRepository)
    : IEmailQueue
{
    private readonly IQueryObject<Domain.EmailMessage.EmailMessage> _emailMessageQuery = emailMessageQuery;
    private readonly ILogger<EmailQueueService> _logger = logger;
    private readonly IRepository<Domain.EmailMessage.EmailMessage> _emailMessageRepository = emailMessageRepository;

    public async Task<ErrorOr<IEnumerable<IEmailQueue.EmailQueueItem>>> GetPendingEmailsAsync(int maxCount = 50)
    {
        _logger.LogInformation("Fetching pending emails from the database.");
        var pendingEmails = (await _emailMessageQuery.ExecuteAsync())
            .AsEnumerable()
            .Where(eM =>
                eM.EmailSendStatus is EmailSendStatus.Pending or EmailSendStatus.Retryable)
            .Take(maxCount)
            .ToList();


        var emailQueueItems = pendingEmails.Select(eM => new IEmailQueue.EmailQueueItem(
            eM.Id.Value,
            eM.EmailRecipient.Value,
            eM.EmailSubject.Value,
            eM.EmailBody.Value,
            eM.EmailAttachments.Select(a => a.Path),
            eM.CreatedAt
        )).ToList();

        _logger.LogInformation("Fetched {Count} pending emails.", emailQueueItems.Count());

        pendingEmails.ForEach(eM =>
        {
            eM.MarkAsInProgress();
            _emailMessageRepository.Update(eM);
        });

        await _emailMessageRepository.CommitAsync();

        return emailQueueItems.ToList();
    }

    public async Task RecordRetryAsync(Guid emailId)
    {
        _logger.LogInformation("Recording retry for email with ID: {EmailId}", emailId);

        var emailMessage = (await _emailMessageQuery.ExecuteAsync())
            .AsEnumerable()
            .SingleOrDefault(eM => eM.Id.Value == emailId);

        if (emailMessage is null)
        {
            _logger.LogWarning("Email with ID: {EmailId} not found.", emailId);
            return;
        }

        emailMessage.RecordRetry();
        _emailMessageRepository.Update(emailMessage);

        await _emailMessageRepository.CommitAsync();
    }

    public async Task MarkAsSentAsync(Guid emailId)
    {
        _logger.LogInformation("Marking email with ID: {EmailId} as sent.", emailId);

        var emailMessage = (await _emailMessageQuery.ExecuteAsync())
            .AsEnumerable()
            .SingleOrDefault(eM => eM.Id.Value == emailId);

        if (emailMessage is null)
        {
            _logger.LogWarning("Email with ID: {EmailId} not found.", emailId);
            return;
        }

        emailMessage.RecordSent();
        _emailMessageRepository.Update(emailMessage);

        await _emailMessageRepository.CommitAsync();
    }

    public async Task EnqueueEmailAsync(
        string recipient,
        string subject,
        string body,
        IEnumerable<string> attachmentPaths)
    {
        var emailMessage = Domain.EmailMessage.EmailMessage.Create(
            recipient,
            subject,
            body,
            attachmentPaths.Select(EmailAttachmentPath.Create));

        await _emailMessageRepository.InsertAsync(emailMessage.Value);

        await _emailMessageRepository.CommitAsync();
    }
}
