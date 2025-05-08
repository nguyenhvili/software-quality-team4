using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using Quartz;
using StocksReportingLibrary.Application.Report;
using StocksReportingLibrary.Configuration;

namespace StocksReportingLibrary.Application.Services.Scheduling;

public class CreateReportJob : IJob
{
    private readonly CreateReportCommandHandler _handler;
    private readonly ILogger<CreateReportJob> _logger;
    private readonly string _downloadPath;

    public CreateReportJob(CreateReportCommandHandler handler, ILoggerFactory loggerFactory, IOptions<ReportSettings> options)
    {
        _handler = handler;
        _logger = loggerFactory.CreateLogger<CreateReportJob>();
        _downloadPath = options.Value.DownloadPath;
    }

    private AsyncRetryPolicy CreateRetryPolicy()
    {
        AsyncRetryPolicy retryPolicy = Policy.Handle<Exception>().WaitAndRetryAsync(
            retryCount: 3,
            sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(3, attempt)),
            onRetry: (exception, timeSpan, retryCount, context) =>
            {
                _logger.LogWarning("Retry {RetryCount} after {TimeSpan} due to error: {ExceptionMessage}",
                retryCount, timeSpan, exception.Message);
            });
        return retryPolicy;
    }
    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Create report job started at {Time}", DateTime.UtcNow);
        var retryPolicy = CreateRetryPolicy();
        try
        {
            await retryPolicy.ExecuteAsync(async () =>
            {
                var result = await _handler.Handle(new CreateReportCommand(_downloadPath, DateTime.Now));

                if (result.IsError)
                {
                    var errors = string.Join(",", result.Errors.Select(error => error.Description));
                    throw new ApplicationException($"CreateReportCommand failed with errors: {errors}");
                }

                _logger.LogInformation("Create report job completed successfully at {Time}", DateTime.UtcNow);
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Create report job failed permanently after retries");
        }
    }
}
