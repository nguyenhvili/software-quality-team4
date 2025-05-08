using ErrorOr;
using StocksReportingLibrary.Application.Services.Persistence;
using Microsoft.Extensions.Logging;

namespace StocksReportingLibrary.Application.Email;

public class CreateEmailCommandHandler
{
    private readonly IRepository<Domain.Email.Email> _repository;
    private readonly ILogger<CreateEmailCommandHandler> _logger;

    public CreateEmailCommandHandler(
        IRepository<Domain.Email.Email> repository,
        ILogger<CreateEmailCommandHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<ErrorOr<CreateEmailCommand.Result>> Handle(CreateEmailCommand command)
    {
        _logger.LogInformation("Creating email with value: {EmailValue}", command.EmailValue);

        var emailResult = Domain.Email.Email.Create(
            command.EmailValue
        );

        if (emailResult.IsError)
        {
            _logger.LogWarning("Email creation failed due to validation errors.");
            foreach (var error in emailResult.Errors)
            {
                _logger.LogDebug("Validation error: {Error}", error);
            }
            return emailResult.Errors;
        }
        var email = emailResult.Value;

        await _repository.InsertAsync(email);
        _logger.LogDebug("Email inserted into repository with ID: {EmailId}", email.Id);

        await _repository.CommitAsync();
        _logger.LogInformation("Email created successfully with ID: {EmailId}", email.Id);

        return new CreateEmailCommand.Result(
            new CreateEmailCommand.CreatedEmail(
                email.Id.Value,
                email.EmailValue.Value
            )
        );
    }
}
