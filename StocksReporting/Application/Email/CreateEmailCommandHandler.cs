using ErrorOr;
using StocksReporting.Application.Services;

namespace StocksReporting.Application.Email;

public class CreateEmailCommandHandler
{
    private readonly IRepository<Domain.Email.Email> _repository;

    public CreateEmailCommandHandler(IRepository<Domain.Email.Email> repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<CreateEmailCommand.Result>> Handle(CreateEmailCommand command)
    {
        var emailResult = Domain.Email.Email.Create(
            command.Id,
            command.EmailValue
        );

        if (emailResult.IsError)
        {
            return emailResult.Errors;
        }
        var email = emailResult.Value;

        await _repository.InsertAsync(email);
        await _repository.CommitAsync();

        return new CreateEmailCommand.Result(
            new CreateEmailCommand.CreatedEmail(
                email.Id.Value,
                email.EmailValue.Value
                )
            );
    }
}
