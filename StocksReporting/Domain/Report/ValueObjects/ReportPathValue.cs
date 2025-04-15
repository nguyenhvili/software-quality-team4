using StocksReporting.Domain.Common;

namespace StocksReporting.Domain.Report.ValueObjects;

public class ReportPathValue : ValueObject
{
    public string PathValue { get; private set; }

    private ReportPathValue(string path)
    {
        PathValue = path;
    }

    public static ReportPathValue Create(string pathValue) => new(pathValue);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return PathValue;
    }
}
