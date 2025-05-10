using ErrorOr;
using JasperFx.Core;
using StocksReportingLibrary.Application.Services.File;
using Microsoft.Extensions.Logging;

namespace StocksReportingLibrary.Infrastructure.File;

public class CsvDownloader : IDownloader
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CsvDownloader> _logger;

    public CsvDownloader(IHttpClientFactory httpClientFactory, ILogger<CsvDownloader> logger)
    {
        _httpClient = httpClientFactory.CreateClient();
        _logger = logger;
    }

    public async Task<ErrorOr<StringReader>> DownloadAsync(string url)
    {
        _logger.LogInformation("Downloading CSV from: {CsvUrl}", url);

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error downloading CSV from: {CsvUrl}", url);
            return Error.Unexpected($"Failed to download CSV: {ex.Message}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error downloading CSV from: {CsvUrl}", url);
            return Error.Unexpected($"Unexpected error: {ex.Message}");
        }

        var stream = await response.Content.ReadAsStreamAsync();
        var reader = new StreamReader(stream);
        var lines = reader.ReadToEnd().Split(Environment.NewLine);
        lines = lines.RemoveAt(lines.Length - 1);

        _logger.LogDebug("CSV downloaded successfully from: {CsvUrl}", url);
        return new StringReader(lines.Join(Environment.NewLine));
    }

}
