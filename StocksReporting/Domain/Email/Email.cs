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

    public static Email Create(Guid id, string email)
    {
        return new Email(EmailId.Create(id), EmailValue.Create(email));
    }
}
