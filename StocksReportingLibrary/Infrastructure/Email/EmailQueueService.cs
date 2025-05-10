using ErrorOr;
using Microsoft.Extensions.Logging;
using StocksReportingLibrary.Application.Services.Email;
using StocksReportingLibrary.Application.Services.Persistence;
using StocksReportingLibrary.Domain.Email.EmailMessage.EmailAttachmentPath;
using StocksReportingLibrary.Domain.Email.EmailMessage.Enums;

namespace StocksReportingLibrary.Infrastructure.Email;

public class EmailQueueService(
    IQueryObject<Domain.Email.EmailMessage.EmailMessage> emailMessageQuery,
    ILogger<EmailQueueService> logger,
    IRepository<Domain.Email.EmailMessage.EmailMessage> emailMessageRepository)
    : IEmailQueue
{
    private readonly IQueryObject<Domain.Email.EmailMessage.EmailMessage> _emailMessageQuery = emailMessageQuery;
    private readonly ILogger<EmailQueueService> _logger = logger;
    private readonly IRepository<Domain.Email.EmailMessage.EmailMessage> _emailMessageRepository = emailMessageRepository;

    public const int MaxRetryCount = 5;

    public async Task<ErrorOr<IEnumerable<IEmailQueue.EmailQueueItem>>> GetPendingEmailsAsync(int maxCount = 50)
    {
        _logger.LogInformation("Fetching pending emails from the database.");
        var pendingEmails = (await _emailMessageQuery.ExecuteAsync())
            .AsEnumerable()
            .Where(eM =>
                eM.EmailSendStatus.Status == EEmailSendStatus.Pending ||
                eM.EmailSendStatus.Status == EEmailSendStatus.Retryable)
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

        if (emailMessage == null)
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

        if (emailMessage == null)
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
        var emailMessage = Domain.Email.EmailMessage.EmailMessage.Create(
            recipient,
            subject,
            body,
            attachmentPaths.Select(EmailAttachmentPath.Create));

        await _emailMessageRepository.InsertAsync(emailMessage.Value);

        await _emailMessageRepository.CommitAsync();
    }
}