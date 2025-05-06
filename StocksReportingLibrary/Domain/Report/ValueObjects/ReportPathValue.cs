using StocksReportingLibrary.Domain.Common;

namespace StocksReportingLibrary.Domain.Report.ValueObjects;

public class ReportPathValue : ValueObject
{
    public string PathValue { get; private set; }

    private ReportPathValue(string pathValue)
    {
        PathValue = pathValue;
    }

    public static ReportPathValue Create(string pathValue) => new(pathValue);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return PathValue;
    }
}
