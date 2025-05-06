using ErrorOr;

namespace StocksReportingLibrary.Application.Services.Email;

public interface IReportEmailService
{

    public Task<ErrorOr<SendReportResult>> SendReportAsync(Guid reportId, List<Guid> emailIds);

    public record SendReportResult(Guid ReportId, List<string> SentTo);
}
