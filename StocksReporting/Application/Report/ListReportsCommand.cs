namespace StocksReporting.Application.Report;

public record ListReportsCommand(int Page, int PageSize)
{
    public record Report(Guid Id, DateTime CreatedAt);

    public record Result(IEnumerable<Report> Reports);

}