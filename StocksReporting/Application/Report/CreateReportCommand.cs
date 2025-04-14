namespace StocksReporting.Application.Report;

public record CreateReportCommand(string Path)
{
    public record CreatedReport(
        Guid Id,
        string Path,
        DateTime CreatedAt
    );

    public record Result(CreatedReport Report);
}
