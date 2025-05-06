namespace StocksReportingLibrary.Application.Report;

public record GetReportCommand(Guid Id)
{
    public record HoldingDetail(
        Guid Id,
        string CompanyName,
        string Ticker,
        long Shares,
        decimal SharesPercent,
        decimal Weight
    );

    public record ReportDetail(
        Guid Id,
        string FilePath,
        DateTime CreatedAt,
        IEnumerable<HoldingDetail> Holdings
    );

    public record Result(ReportDetail Report);
}
