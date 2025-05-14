using ErrorOr;
using StocksReportingLibrary.Domain.Common;
using StocksReportingLibrary.Domain.Email.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace StocksReportingLibrary.Domain.Email;

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
        if (string.IsNullOrWhiteSpace(email))
        {
            return Error.Validation("The email is empty!");
        }
        var emailValidation = new EmailAddressAttribute();
        if (!emailValidation.IsValid(email))
        {
            return Error.Validation("The email is not in valid format!");
        }
        return new Email(EmailId.CreateUnique(), EmailValue.Create(email));
    }
}
