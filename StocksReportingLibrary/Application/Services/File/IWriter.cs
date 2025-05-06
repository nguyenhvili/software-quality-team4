namespace StocksReportingLibrary.Application.Services.File;

public interface IWriter
{

    public void Write(Domain.Report.Report report);

}
