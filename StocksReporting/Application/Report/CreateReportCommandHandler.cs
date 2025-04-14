using ErrorOr;
using StocksReporting.Application.Services;

namespace StocksReporting.Application.Report;

public class CreateReportCommandHandler
{
    private readonly IRepository<Domain.Report.Report> _repository;

    public CreateReportCommandHandler(IRepository<Domain.Report.Report> repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<CreateReportCommand.Result>> Handle(CreateReportCommand command)
    {
        var reportResult = Domain.Report.Report.Create(
            command.Path,
            DateTime.UtcNow
        );

        if (reportResult.IsError)
        {
            return reportResult.Errors;
        }

        var report = reportResult.Value;

        await _repository.InsertAsync(report);
        await _repository.CommitAsync();

        return new CreateReportCommand.Result(
            new CreateReportCommand.CreatedReport(
                report.Id.Value,
                report.FilePath,
                report.CreatedAt
            )
        );
    }
}
