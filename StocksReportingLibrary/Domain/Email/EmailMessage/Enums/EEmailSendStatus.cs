namespace StocksReportingLibrary.Domain.Email.EmailMessage.Enums;

public enum EEmailSendStatus
{
    Pending = 0,
    Retryable = 1,
    InProgress = 2,
    Sent = 3,
    Failed = 4,
}