using StocksReporting.Domain.Common;

namespace StocksReporting.Domain.Report.Holding.ValueObjects;

public class Weight : ValueObject
{
    public decimal Value { get; set; }

    private Weight(decimal value)
    {
        this.Value = value;
    }

    public static Weight Create(decimal value) => new(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
