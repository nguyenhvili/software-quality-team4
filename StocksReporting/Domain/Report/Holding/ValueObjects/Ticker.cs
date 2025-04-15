using StocksReporting.Domain.Common;

namespace StocksReporting.Domain.Report.Holding.ValueObjects;

public class Ticker : ValueObject
{
    public string Value { get; set; }

    private Ticker(string value)
    {
        this.Value = value;
    }

    public static Company Create(string value) => new(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
