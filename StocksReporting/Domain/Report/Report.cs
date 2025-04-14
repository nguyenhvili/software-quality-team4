using ErrorOr;
using StocksReporting.Domain.Common;
using StocksReporting.Domain.Report.ValueObjects;

namespace StocksReporting.Domain.Report;

public class Report : AggregateRoot<ReportId>
{
    public string FilePath { get; private set; } = string.Empty;
    public DateTime CreatedAt { get; private set; }

    public Report()
    {
    }

    private Report(ReportId reportId, string filePath, DateTime createdAt) : base(reportId)
    {
        FilePath = filePath;
        CreatedAt = createdAt;
    }

    public static ErrorOr<Report> Create(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            return Error.Validation("Report.FilePath", "File path cannot be empty.");
        }

        return new Report(ReportId.CreateUnique(), filePath, DateTime.UtcNow);
    }
}