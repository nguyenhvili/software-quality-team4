using ErrorOr;
using Microsoft.AspNetCore.Http;
using StocksReportingLibrary.Application.Email;
using Wolverine;
using Wolverine.Http;
using Microsoft.Extensions.Logging;

namespace StocksReportingLibrary.Presentation.Email;

public class DeleteEmailEndpoint
{
    private readonly ILogger<DeleteEmailEndpoint> _logger;

    public DeleteEmailEndpoint(ILogger<DeleteEmailEndpoint> logger)
    {
        _logger = logger;
    }

    [WolverineDelete("emails/{id}")]
    public async static Task<IResult> DeleteEmailAsync(Guid id, IMessageBus sender, ILogger<DeleteEmailEndpoint> logger)
    {
        logger.LogInformation("Received request to delete email with Id: {Id}", id);

        var command = new DeleteEmailCommand(id);

        var result = await sender.InvokeAsync<ErrorOr<DeleteEmailCommand.Result>>(command);

        return result.Match(
            _ =>
            {
                logger.LogInformation("Email with Id: {Id} deleted successfully", id);
                return Results.NoContent();
            },
            errors =>
            {
                logger.LogWarning("Failed to delete email with Id: {Id}. Errors: {Errors}", id, errors);
                return Results.BadRequest(errors.Select(e => e.Code));
            }
        );
    }
}
