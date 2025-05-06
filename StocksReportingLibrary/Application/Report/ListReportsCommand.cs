using StocksReportingLibrary.Application.Common;

namespace StocksReportingLibrary.Application.Report;

public record ListReportsCommand(Paging Paging)
{
    public record Report(Guid Id, DateTime CreatedAt);

    public record Result(IEnumerable<Report> Reports);

}
