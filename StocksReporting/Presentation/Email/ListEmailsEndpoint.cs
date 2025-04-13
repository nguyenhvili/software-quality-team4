using ErrorOr;
using StocksReporting.Application.Email;
using Wolverine;
using Wolverine.Http;

namespace StocksReporting.Presentation.Email;

public class ListEmailsEndpoint
{
    [WolverineGet("emails")]
    public static async Task<IResult> ListEmailsAsync(Request request, IMessageBus sender)
    {
        var command = new ListEmailsCommand(request.Page, request.PageSize);

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

    public record Request(int Page, int PageSize);

    public record Response(IEnumerable<Response.Email> Emails)
    {
        public record Email(Guid Id, string EmailValue);
    }
}