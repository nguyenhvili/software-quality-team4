using StocksReportingLibrary.Application.Common;

namespace StocksReportingLibrary.Application.Email;

public record ListEmailsCommand(Paging Paging)
{
    public record Result(IEnumerable<Email> Emails);

    public record Email(Guid Id, string EmailValue);
}
