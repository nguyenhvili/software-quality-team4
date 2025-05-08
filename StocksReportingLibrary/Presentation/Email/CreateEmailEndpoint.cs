using ErrorOr;
using Microsoft.AspNetCore.Http;
using StocksReportingLibrary.Application.Email;
using Wolverine;
using Wolverine.Http;
using Microsoft.Extensions.Logging;

namespace StocksReportingLibrary.Presentation.Email;

public class CreateEmailEndpoint
{
    private readonly ILogger<CreateEmailEndpoint> _logger;

    public CreateEmailEndpoint(ILogger<CreateEmailEndpoint> logger)
    {
        _logger = logger;
    }

    [WolverinePost("emails")]
    public async static Task<IResult> CreateEmailAsync(Request request, IMessageBus sender, ILogger<CreateEmailEndpoint> logger)
    {
        logger.LogInformation("Received request to create email with Value: {Name}", request.Name);

        var command = new CreateEmailCommand(request.Name);

        var result = await sender.InvokeAsync<ErrorOr<CreateEmailCommand.Result>>(command);

        return result.Match(
            value =>
            {
                logger.LogInformation("Email created successfully. Id: {Id}, EmailValue: {EmailValue}", value.Email.Id, value.Email.EmailValue);
                return Results.Ok(
                    new Response(
                        new Response.CreatedEmail(
                            value.Email.Id,
                            value.Email.EmailValue
                        )
                    )
                );
            },
            errors =>
            {
                logger.LogWarning("Failed to create email. Errors: {Errors}", errors);
                return Results.BadRequest(errors.Select(e => e.Code));
            }
        );
    }

    public record Request(string Name);

    public record Response(Response.CreatedEmail Email)
    {
        public record CreatedEmail(Guid Id, string EmailValue);
    }
}
