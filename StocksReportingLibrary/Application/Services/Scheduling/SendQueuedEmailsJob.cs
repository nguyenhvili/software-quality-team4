using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Quartz;
using StocksReportingLibrary.Application.Services.Email;

namespace StocksReportingLibrary.Application.Services.Scheduling;

[DisallowConcurrentExecution]
public class SendQueuedEmailsJob : IJob
{
    private readonly IEmailSender _emailSender;
    private readonly IEmailQueue _emailQueue;
    private readonly ILogger<SendQueuedEmailsJob> _logger;
    private readonly IAsyncPolicy _retryPolicy;

    public SendQueuedEmailsJob(
        IEmailSender emailSender,
        IEmailQueue emailQueue,
        ILogger<SendQueuedEmailsJob> logger,
        [FromKeyedServices("MailRetryPolicy")]
        ResiliencePipeline pipeline)
    {
        _emailSender = emailSender;
        _emailQueue = emailQueue;
        _logger = logger;
        _retryPolicy = pipeline.AsAsyncPolicy();

    }

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Attempting to send queued emails at {Time}", DateTime.UtcNow);

        await _retryPolicy.ExecuteAsync(async () =>
        {
            var pendingEmails = await _emailQueue.GetPendingEmailsAsync();
            if (pendingEmails.IsError)
            {
                var errors = string.Join(",", pendingEmails.Errors.Select(error => error.Description));
                _logger.LogError("Failed to fetch pending emails: {Errors}", errors);
                return;
            }

            foreach (var email in pendingEmails.Value)
            {
                var sendResult = await _retryPolicy.ExecuteAndCaptureAsync(async () => await _emailSender.SendEmailAsync(
                    email.Recipient,
                    email.Subject,
                    email.Body,
                    email.AttachmentPaths.ToArray()
                ));

                if (sendResult.Outcome == OutcomeType.Successful)
                {
                    await _emailQueue.MarkAsSentAsync(email.Id);
                    _logger.LogInformation("Email sent successfully to {Recipient}", email.Recipient);
                }
                else
                {
                    _logger.LogWarning("Failed to send email: {Errors}", sendResult.FinalException?.Message);
                    await _emailQueue.RecordRetryAsync(email.Id);
                }
            }
        });

        _logger.LogInformation("Send queued emails job completed successfully at {Time}", DateTime.UtcNow);

    }
}
