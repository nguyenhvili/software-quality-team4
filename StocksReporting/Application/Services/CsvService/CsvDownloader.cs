using ErrorOr;
using JasperFx.Core;
using System.Net.Http;

namespace StocksReporting.Application.Services.CsvService;

public class CsvDownloader
{
    private readonly HttpClient _httpClient;

    public CsvDownloader(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task<ErrorOr<StringReader>> Download(string csvUrl)
    {
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync(csvUrl);
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
