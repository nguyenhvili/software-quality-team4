using StocksReportingLibrary.Domain.Common;

namespace StocksReportingLibrary.Domain.Report.ValueObjects;

public class ReportId : ValueObject
{
    public Guid Value { get; private set; }

    private ReportId(Guid value)
    {
        Value = value;
    }

    public static ReportId Create(Guid id) => new(id);
    public static ReportId CreateUnique() => new(Guid.NewGuid());

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
