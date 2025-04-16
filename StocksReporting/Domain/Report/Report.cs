using ErrorOr;
using StocksReporting.Domain.Common;
using StocksReporting.Domain.Report.Holding;
using StocksReporting.Domain.Report.ValueObjects;

namespace StocksReporting.Domain.Report;

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

    public static ErrorOr<Report> Create(string filePath, IEnumerable<Holding.Holding> holdings, DateTime? createdAt = null)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            return Error.Validation("Report.FilePath", "File path cannot be empty.");
        }

        var time = createdAt ?? DateTime.UtcNow;
        return new Report(ReportId.CreateUnique(), ReportPathValue.Create(filePath), time, holdings);
    }
}
