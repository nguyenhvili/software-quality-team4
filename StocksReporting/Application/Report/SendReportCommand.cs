namespace StocksReporting.Application.Report;

public record SendReportCommand(string FilePath, List<Guid> EmailIds);
