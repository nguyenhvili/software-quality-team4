namespace StocksReporting.Application.Email;

public record CreateEmailCommand(Guid Id, string EmailValue)
{
    public record CreatedEmail(
        Guid Id,
        string EmailValue
    );

    public record Result(CreatedEmail Email);
}
