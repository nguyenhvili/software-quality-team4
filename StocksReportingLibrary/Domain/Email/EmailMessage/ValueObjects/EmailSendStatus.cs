using StocksReportingLibrary.Domain.Common;
using StocksReportingLibrary.Domain.Email.EmailMessage.Enums;

namespace StocksReportingLibrary.Domain.Email.EmailMessage.ValueObjects;

public class EmailSendStatus : ValueObject
{
    public EEmailSendStatus Status { get; private set; }

    private EmailSendStatus(EEmailSendStatus status)
    {
        Status = status;
    }

    public static EmailSendStatus Create(EEmailSendStatus status)
    {

        return new EmailSendStatus(status);
    }

    public static EmailSendStatus CreateDefault()
    {
        return new EmailSendStatus(EEmailSendStatus.Pending);
    }

    public void UpdateStatus(EEmailSendStatus status)
    {
        Status = status;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Status;
    }
}