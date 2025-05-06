using ErrorOr;
using Microsoft.AspNetCore.Http;
using StocksReportingLibrary.Application.Common;
using StocksReportingLibrary.Application.Email;
using Wolverine;
using Wolverine.Http;

namespace StocksReportingLibrary.Presentation.Email;

public class ListEmailsEndpoint
{
    [WolverineGet("emails")]
    public static async Task<IResult> ListEmailsAsync(int page, int pageSize, IMessageBus sender)
    {
        var pagingResult = Paging.Create(page, pageSize);
        if (pagingResult.IsError)
        {
            return Results.BadRequest(pagingResult.Errors.Select(e => e.Code));
        }
        
        var command = new ListEmailsCommand(pagingResult.Value);

        var result = await sender.InvokeAsync<ErrorOr<ListEmailsCommand.Result>>(command);

        return result.Match(
            value => Results.Ok(
                new Response(
                    value.Emails.Select(email => new Response.Email(email.Id, email.EmailValue))
                )
            ),
            errors => Results.BadRequest(errors.Select(e => e.Code))
        );
    }

    public record Response(IEnumerable<Response.Email> Emails)
    {
        public record Email(Guid Id, string EmailValue);
    }
}
