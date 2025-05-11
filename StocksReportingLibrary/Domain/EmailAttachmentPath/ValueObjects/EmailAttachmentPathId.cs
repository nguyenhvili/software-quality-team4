using StocksReportingLibrary.Domain.Common;

namespace StocksReportingLibrary.Domain.EmailAttachmentPath.ValueObjects;
public class EmailAttachmentPathId : ValueObject
{
    public Guid Value { get; private set; }

    private EmailAttachmentPathId(Guid value)
    {
        Value = value;
    }

    public static EmailAttachmentPathId Create(Guid id) => new(id);
    public static EmailAttachmentPathId CreateUnique() => new(Guid.NewGuid());

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
