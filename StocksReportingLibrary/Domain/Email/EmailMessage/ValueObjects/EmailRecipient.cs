using StocksReportingLibrary.Domain.Common;

namespace StocksReportingLibrary.Domain.Email.EmailMessage.ValueObjects;

public class EmailRecipient : ValueObject
{
    public string Value { get; private set; }

    private EmailRecipient(string value)
    {
        Value = value;
    }

    public static EmailRecipient Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("The email recipient is empty!");
        }

        return new EmailRecipient(email);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }
}