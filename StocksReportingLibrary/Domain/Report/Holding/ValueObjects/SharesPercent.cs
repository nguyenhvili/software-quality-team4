using StocksReportingLibrary.Domain.Common;

namespace StocksReportingLibrary.Domain.Report.Holding.ValueObjects;

public class SharesPercent : ValueObject
{
    public decimal Value { get; private set; }

    private SharesPercent(decimal value)
    {
        this.Value = value;
    }

    public static SharesPercent Create(decimal value) => new(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
