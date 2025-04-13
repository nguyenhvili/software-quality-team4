using ErrorOr;
using StocksReporting.Application.Services;

namespace StocksReporting.Application.Email;

public class ListEmailsCommandHandler
{
    private readonly IQueryObject<Domain.Email.Email> _queryObject;

    public ListEmailsCommandHandler(IQueryObject<Domain.Email.Email> queryObject)
    {
        _queryObject = queryObject;
    }

    public async Task<ErrorOr<ListEmailsCommand.Result>> Handle(ListEmailsCommand command)
    {
        var emails = await _queryObject.Page(command.Page, command.PageSize).ExecuteAsync();

        var result = emails.Select(email => new ListEmailsCommand.Email(email.Id.Value, email.EmailValue.Value));

        return new ListEmailsCommand.Result(result);
    }
}