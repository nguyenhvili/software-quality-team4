using StocksReportingLibrary.Domain.Common;

namespace StocksReportingLibrary.Domain.EmailMessage.ValueObjects;

public class EmailMessageId : ValueObject
{
    public Guid Value { get; private set; }

    private EmailMessageId(Guid value)
    {
        Value = value;
    }

    public static EmailMessageId Create(Guid id) => new(id);
    public static EmailMessageId CreateUnique() => new(Guid.NewGuid());

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
