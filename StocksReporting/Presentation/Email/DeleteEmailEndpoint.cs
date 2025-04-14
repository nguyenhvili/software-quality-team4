using ErrorOr;
using StocksReporting.Application.Email;
using Wolverine;
using Wolverine.Http;

namespace StocksReporting.Presentation.Email;

public class DeleteEmailEndpoint
{
    [WolverineDelete("emails/{id}")]
    public static async Task<IResult> DeleteEmailAsync(Guid id, IMessageBus sender)
    {
        var command = new DeleteEmailCommand(id);

        var result = await sender.InvokeAsync<ErrorOr<DeleteEmailCommand.Result>>(command);

        return result.Match(
            _ => Results.NoContent(),
            errors => Results.BadRequest(errors.Select(e => e.Code))
        );
    }
}
