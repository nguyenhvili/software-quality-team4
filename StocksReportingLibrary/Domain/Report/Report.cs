using ErrorOr;
using StocksReportingLibrary.Domain.Common;
using StocksReportingLibrary.Domain.Report.Holding;
using StocksReportingLibrary.Domain.Report.ValueObjects;

namespace StocksReportingLibrary.Domain.Report;

public class Report : AggregateRoot<ReportId>
{
    public ReportPathValue ReportPathValue { get; private set; }
    public DateTime CreatedAt { get; private set; }
    private List<Holding.Holding> _holdings = new();
    public IReadOnlyList<Holding.Holding> Holdings => _holdings.AsReadOnly();

    public Report()
    {
    }

    private Report(ReportId reportId, ReportPathValue filePath, DateTime createdAt, IEnumerable<Holding.Holding> holdings) : base(reportId)
    {
        ReportPathValue = filePath;
        CreatedAt = createdAt;
        _holdings = holdings.ToList();
    }

    public static ErrorOr<Report> Create(string filePath, IEnumerable<Holding.Holding> holdings, DateTime createdAt)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            return Error.Validation("Report.FilePath", "File path cannot be empty.");
        }

        if (holdings is null)
        {
            return Error.Validation("Report.Holdings", "Holdings is null.");
        }

        return new Report(ReportId.CreateUnique(), ReportPathValue.Create(filePath), createdAt, holdings);
    }
}
