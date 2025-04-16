using StocksReporting.Domain.Common;

namespace StocksReporting.Domain.Report.Holding.ValueObjects;

public class Ticker : ValueObject
{
    public string Value { get; private set; }

    private Ticker(string value)
    {
        this.Value = value;
    }

    public static Ticker Create(string value) => new(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
