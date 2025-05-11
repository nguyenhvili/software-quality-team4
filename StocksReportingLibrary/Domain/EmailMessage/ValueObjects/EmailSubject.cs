using StocksReportingLibrary.Domain.Common;

namespace StocksReportingLibrary.Domain.EmailMessage.ValueObjects;

public class EmailSubject : ValueObject
{
    public string Value { get; private set; }

    private EmailSubject(string value)
    {
        Value = value;
    }

    public static EmailSubject Create(string subject)
    {
        if (string.IsNullOrWhiteSpace(subject))
        {
            throw new ArgumentException("The subject is empty!");
        }

        return new EmailSubject(subject);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}
