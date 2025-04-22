namespace StocksReporting.Application.Report;

public record CreateReportCommand(string DownloadPath, DateTime CreatedAt)
{
    public record CreatedReport(
        Guid Id,
        string FilePath,
        DateTime CreatedAt
    );

    public record Result(CreatedReport Report);
}
