using ErrorOr;
using StocksReportingLibrary.Application.Services.Persistence;
using Microsoft.Extensions.Logging;

namespace StocksReportingLibrary.Application.Email;

public class ListEmailsCommandHandler
{
    private readonly IQueryObject<Domain.Email.Email> _queryObject;
    private readonly ILogger<ListEmailsCommandHandler> _logger;

    public ListEmailsCommandHandler(
        IQueryObject<Domain.Email.Email> queryObject,
        ILogger<ListEmailsCommandHandler> logger)
    {
        _queryObject = queryObject;
        _logger = logger;
    }

    public async Task<ErrorOr<ListEmailsCommand.Result>> Handle(ListEmailsCommand command)
    {
        _logger.LogInformation("Listing emails with Page: {Page}, PageSize: {PageSize}",
            command.Paging.Page, command.Paging.PageSize);

        var emails = await _queryObject.OrderBy(email => email.EmailValue.Value).Page(command.Paging).ExecuteAsync();

        _logger.LogDebug("Retrieved {EmailCount} emails from the database.", emails.Count());

        var result = emails.Select(email => new ListEmailsCommand.Email(email.Id.Value, email.EmailValue.Value));

        _logger.LogInformation("Returning {ResultCount} emails in the list.", result.Count());

        return new ListEmailsCommand.Result(result);
    }
}
