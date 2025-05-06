using StocksReportingLibrary.Domain.Common;

namespace StocksReportingLibrary.Domain.Report.Holding.ValueObjects;

public class Weight : ValueObject
{
    public decimal Value { get; private set; }

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
