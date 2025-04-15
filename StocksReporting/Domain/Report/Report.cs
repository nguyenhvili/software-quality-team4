using ErrorOr;
using StocksReporting.Domain.Common;
using StocksReporting.Domain.Report.ValueObjects;

namespace StocksReporting.Domain.Report;

public class Report : AggregateRoot<ReportId>
{
    public ReportPathValue ReportPathValue { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Report()
    {
    }

    private Report(ReportId reportId, ReportPathValue filePath, DateTime createdAt) : base(reportId)
    {
        ReportPathValue = filePath;
        CreatedAt = createdAt;
    }

    public static ErrorOr<Report> Create(string filePath, DateTime? createdAt = null)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            return Error.Validation("Report.FilePath", "File path cannot be empty.");
        }

        var time = createdAt ?? DateTime.UtcNow;
        return new Report(ReportId.CreateUnique(), ReportPathValue.Create(filePath), time);
    }
}
