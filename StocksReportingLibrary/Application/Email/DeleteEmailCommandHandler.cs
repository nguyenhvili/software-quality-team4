using ErrorOr;
using StocksReportingLibrary.Application.Services.Persistence;
using StocksReportingLibrary.Domain.Email.ValueObjects;
using Microsoft.Extensions.Logging;

namespace StocksReportingLibrary.Application.Email;

public class DeleteEmailCommandHandler
{
    private readonly IRepository<Domain.Email.Email> _repository;
    private readonly IQueryObject<Domain.Email.Email> _queryObject;
    private readonly ILogger<DeleteEmailCommandHandler> _logger;

    public DeleteEmailCommandHandler(
        IRepository<Domain.Email.Email> repository,
        IQueryObject<Domain.Email.Email> queryObject,
        ILogger<DeleteEmailCommandHandler> logger)
    {
        _repository = repository;
        _queryObject = queryObject;
        _logger = logger;
    }

    public async Task<ErrorOr<DeleteEmailCommand.Result>> Handle(DeleteEmailCommand command)
    {
        _logger.LogInformation("Deleting email with ID: {EmailId}", command.Id);

        var email = (await _queryObject.Filter(email => email.Id == EmailId.Create(command.Id)).ExecuteAsync())
            .SingleOrDefault();
        if (email is null)
        {
            _logger.LogWarning("Email with ID {EmailId} not found.", command.Id);
            return Error.NotFound("Email not found.");
        }

        try
        {
            var deleted = await _repository.RemoveAsync(email.Id);
            if (!deleted)
            {
                _logger.LogError("Failed to delete email with ID: {EmailId}", command.Id);
                return Error.Validation("An error occurred while deleting the email.");
            }
            await _repository.CommitAsync();
            _logger.LogInformation("Email with ID {EmailId} deleted successfully.", command.Id);
            return new DeleteEmailCommand.Result();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting email with ID: {EmailId}", command.Id);
            return Error.Failure("An unexpected error occurred while deleting the email.");
        }
    }
}
