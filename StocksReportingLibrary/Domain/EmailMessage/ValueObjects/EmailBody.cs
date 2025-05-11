using StocksReportingLibrary.Domain.Common;

namespace StocksReportingLibrary.Domain.EmailMessage.ValueObjects;

public class EmailBody : ValueObject
{
    public string Value { get; private set; }

    private EmailBody(string value)
    {
        Value = value;
    }

    public static EmailBody Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("The email body is empty!");
        }

        return new EmailBody(email);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}