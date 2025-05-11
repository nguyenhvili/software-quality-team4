namespace StocksReportingLibrary.Domain.EmailMessage.Enums;

public enum EmailSendStatus
{
    Pending = 0,
    Retryable = 1,
    InProgress = 2,
    Sent = 3,
    Failed = 4,
}