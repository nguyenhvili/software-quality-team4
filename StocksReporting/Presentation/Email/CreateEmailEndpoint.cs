using ErrorOr;
using StocksReporting.Application.Email;
using Wolverine;
using Wolverine.Http;

namespace StocksReporting.Presentation.Email;

public class CreateEmailEndpoint
{
    [WolverinePost("emails")]
    public static async Task<IResult> CreateEmailAsync(Request request, IMessageBus sender)
    {
        var command = new CreateEmailCommand(request.Name);

        var result = await sender.InvokeAsync<ErrorOr<CreateEmailCommand.Result>>(command);

        return result.Match(
            value => Results.Ok(
                new Response(
                    new Response.CreatedEmail(
                        value.Email.Id,
                        value.Email.EmailValue
                    )
                )
            ),
            errors => Results.BadRequest(errors.Select(e => e.Code))
        );
    }

    public record Request(string Name);

    public record Response(Response.CreatedEmail Email)
    {
        public record CreatedEmail(Guid Id, string EmailValue);
    }
}