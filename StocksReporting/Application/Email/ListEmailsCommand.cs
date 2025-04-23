using StocksReporting.Application.Common;

namespace StocksReporting.Application.Email;

public record ListEmailsCommand(Paging Paging)
{
    public record Result(IEnumerable<Email> Emails);

    public record Email(Guid Id, string EmailValue);
}
