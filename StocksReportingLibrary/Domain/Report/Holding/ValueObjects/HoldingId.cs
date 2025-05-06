using StocksReportingLibrary.Domain.Common;
using StocksReportingLibrary.Domain.Email.ValueObjects;

namespace StocksReportingLibrary.Domain.Report.Holding.ValueObjects;

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
