namespace StocksReportingLibrary.Application.Report;

public record SendReportCommand(Guid ReportId, List<Guid> EmailIds);
