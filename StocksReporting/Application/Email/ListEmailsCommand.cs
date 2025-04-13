namespace StocksReporting.Application.Email;

public record ListEmailsCommand(int Page, int PageSize)
{
    public record Result(IEnumerable<Email> Emails);

    public record Email(Guid Id, string EmailValue);
}
