using ErrorOr;
using JasperFx.Core;

namespace StocksReportingLibrary.Application.Services.File;

public interface IDownloader
{

    public Task<ErrorOr<StringReader>> DownloadAsync(string url);
}
