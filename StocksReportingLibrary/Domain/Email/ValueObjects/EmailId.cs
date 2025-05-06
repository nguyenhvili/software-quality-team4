using StocksReportingLibrary.Domain.Common;

namespace StocksReportingLibrary.Domain.Email.ValueObjects;

public class EmailId : ValueObject
{
    public Guid Value { get; private set; }

    private EmailId(Guid value)
    {
        Value = value;
    }

    public static EmailId Create(Guid id) => new(id);
    public static EmailId CreateUnique() => new(Guid.NewGuid());

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
