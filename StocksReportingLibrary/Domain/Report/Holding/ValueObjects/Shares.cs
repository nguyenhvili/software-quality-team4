using StocksReportingLibrary.Domain.Common;

namespace StocksReportingLibrary.Domain.Report.Holding.ValueObjects;

public class Shares : ValueObject
{
    public long Value { get; private set; }

    private Shares(long value)
    {
        this.Value = value;
    }

    public static Shares Create(long value) => new(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
