using StocksReporting.Domain.Common;

namespace StocksReporting.Domain.Email.ValueObjects;

public class EmailValue : ValueObject
{
    public string Value { get; private set; }

    private EmailValue(string value)
    {
        Value = value;
    }

    public static EmailValue Create(string value) => new(value);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
