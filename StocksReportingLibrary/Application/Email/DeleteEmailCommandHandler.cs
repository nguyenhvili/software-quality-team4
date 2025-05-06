using ErrorOr;
using StocksReportingLibrary.Application.Services.Persistence;
using StocksReportingLibrary.Domain.Email.ValueObjects;

namespace StocksReportingLibrary.Application.Email;

public class DeleteEmailCommandHandler
{
    private readonly IRepository<Domain.Email.Email> _repository;
    private readonly IQueryObject<Domain.Email.Email> _queryObject;

    public DeleteEmailCommandHandler(IRepository<Domain.Email.Email> repository, IQueryObject<Domain.Email.Email> queryObject)
    {
        _repository = repository;
        _queryObject = queryObject;
    }

    public async Task<ErrorOr<DeleteEmailCommand.Result>> Handle(DeleteEmailCommand command)
    {
        var email = (await _queryObject.Filter(email => email.Id == EmailId.Create(command.Id)).ExecuteAsync())
            .SingleOrDefault();
        if (email is null)
        {
            return Error.NotFound("Email not found.");
        }

        var deleted = await _repository.RemoveAsync(email.Id);
        if (!deleted)
        {
            return Error.Validation("An error occurred while deleting the email.");
        }
        await _repository.CommitAsync();

        return new DeleteEmailCommand.Result();
    }
}
