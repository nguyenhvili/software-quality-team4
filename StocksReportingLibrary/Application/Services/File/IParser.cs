using StocksReportingLibrary.Domain.Report.Holding;
using ErrorOr;

namespace StocksReportingLibrary.Application.Services.File;

public interface IParser
{

    public Task<ErrorOr<List<Holding>>> ParseAsync(string csvUrl);
}
