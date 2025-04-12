using ErrorOr;
using StocksReporting.Domain.Common;
using StocksReporting.Domain.Email.ValueObjects;

namespace StocksReporting.Domain.Email;

public class Email : AggregateRoot<EmailId>
{
    public EmailValue EmailValue { get; private set; }

    public Email()
    {

    }

    private Email(EmailId emailId, EmailValue emailValue) : base(emailId)
    {
        EmailValue = emailValue;
    }

    public static ErrorOr<Email> Create(string email)
    {
        if (email == null || email.Length == 0)
        {
            return Error.Validation("The email is empty!");
        }
        return new Email(EmailId.CreateUnique(), EmailValue.Create(email));
    }
}
