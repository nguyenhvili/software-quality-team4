using Microsoft.Extensions.Options;
using Quartz;
using StocksReporting.Application.Report;
using StocksReporting.Configuration;

namespace StocksReporting.Application.Services.Scheduling;

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

    public async Task Execute(IJobExecutionContext context)
    {
        _logger.LogInformation("Create report job started at {Time}", DateTime.UtcNow);

        var result = await _handler.Handle(new CreateReportCommand(_downloadPath, DateTime.Now));
        if (result.IsError)
        {
            _logger.LogError("Create report job failed: {Errors}", result.Errors);
            return;
        }

        _logger.LogInformation("Create report job completed successfully at {Time}", DateTime.UtcNow);
    }
}
