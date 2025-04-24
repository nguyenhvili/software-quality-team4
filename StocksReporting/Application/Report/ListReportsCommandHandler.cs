using ErrorOr;
using StocksReporting.Application.Services;

namespace StocksReporting.Application.Report;

public class ListReportsCommandHandler(IQueryObject<Domain.Report.Report> queryObject)
{
    public async Task<ErrorOr<ListReportsCommand.Result>> Handle(ListReportsCommand command)
    {
        var reports = await queryObject.OrderBy(r => r.CreatedAt, false).Page(command.Paging).ExecuteAsync();
        var result = reports.Select(r => new ListReportsCommand.Report(r.Id.Value, r.CreatedAt));

        return new ListReportsCommand.Result(result);
    }
}
