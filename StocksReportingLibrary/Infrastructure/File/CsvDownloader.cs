using ErrorOr;
using JasperFx.Core;
using StocksReportingLibrary.Application.Services.File;

namespace StocksReportingLibrary.Infrastructure.File;

public class CsvDownloader : IDownloader
{
    private readonly HttpClient _httpClient;

    public CsvDownloader(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task<ErrorOr<StringReader>> DownloadAsync(string url)
    {
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync(url);
        }
        catch (Exception ex)
        {
            return Error.Unexpected(ex.Message);
        }

        var stream = await response.Content.ReadAsStreamAsync();
        var reader = new StreamReader(stream);
        var lines = reader.ReadToEnd().Split(Environment.NewLine);
        lines = lines.RemoveAt(lines.Length - 1);
        return new StringReader(lines.Join(Environment.NewLine));
    }

}
