using ErrorOr;
using Microsoft.AspNetCore.Http;
using StocksReportingLibrary.Application.Common;
using StocksReportingLibrary.Application.Email;
using Wolverine;
using Wolverine.Http;
using Microsoft.Extensions.Logging;

namespace StocksReportingLibrary.Presentation.Email;

public class ListEmailsEndpoint
{
    private readonly ILogger<ListEmailsEndpoint> _logger;

    public ListEmailsEndpoint(ILogger<ListEmailsEndpoint> logger)
    {
        _logger = logger;
    }

    [WolverineGet("emails")]
    public async static Task<IResult> ListEmailsAsync(int page, int pageSize, IMessageBus sender, ILogger<ListEmailsEndpoint> logger)
    {
        logger.LogInformation("Received request to list emails. Page: {Page}, PageSize: {PageSize}", page, pageSize);

        var pagingResult = Paging.Create(page, pageSize);
        if (pagingResult.IsError)
        {
            logger.LogWarning("Invalid paging parameters. Errors: {Errors}", pagingResult.Errors);
            return Results.BadRequest(pagingResult.Errors.Select(e => e.Code));
        }

        var command = new ListEmailsCommand(pagingResult.Value);

        var result = await sender.InvokeAsync<ErrorOr<ListEmailsCommand.Result>>(command);

        return result.Match(
            value =>
            {
                logger.LogInformation("Emails retrieved successfully. Count: {Count}", value.Emails.Count());
                return Results.Ok(
                    new Response(
                        value.Emails.Select(email => new Response.Email(email.Id, email.EmailValue))
                    )
                );
            },
            errors =>
            {
                logger.LogWarning("Failed to retrieve emails. Errors: {Errors}", errors);
                return Results.BadRequest(errors.Select(e => e.Code));
            }
        );
    }

    public record Response(IEnumerable<Response.Email> Emails)
    {
        public record Email(Guid Id, string EmailValue);
    }
}
