using StocksReportingLibrary.Domain.Common;

namespace StocksReportingLibrary.Domain.EmailMessage.ValueObjects;

public class RetryCount : ValueObject
{
    public int Value { get; private set; }

    private RetryCount(int value)
    {
        Value = value;
    }

    public static RetryCount Create(int count) => new(count);

    public void Increment()
    {
        Value++;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
