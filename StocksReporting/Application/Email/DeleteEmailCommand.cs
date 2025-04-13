namespace StocksReporting.Application.Email;

public record DeleteEmailCommand(Guid Id)
{
    public record Result();
}
