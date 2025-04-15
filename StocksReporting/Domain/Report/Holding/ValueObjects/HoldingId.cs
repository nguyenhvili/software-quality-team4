using StocksReporting.Domain.Common;
using StocksReporting.Domain.Email.ValueObjects;

namespace StocksReporting.Domain.Report.Holding.ValueObjects;

public class HoldingId : ValueObject
{
    public Guid Value { get; private set; }

    private HoldingId(Guid value)
    {
        Value = value;
    }

    public static HoldingId Create(Guid id) => new(id);
    public static HoldingId CreateUnique() => new(Guid.NewGuid());

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
