using ErrorOr;
using StocksReportingLibrary.Domain.Report;
using StocksReportingLibrary.Domain.Report.Holding;

namespace Test.Unit.Domain;

public class ReportUnitTest
{
    [Fact]
    public void ReportCreate_ValidInput_CorrectValidation()
    {
        var filePath = "file045.csv";
        var createdAt = DateTime.Now;
        var holdings = new List<Holding>();

        new When()
            .UseValidFilePath(filePath)
            .UseValidCreatedAt(createdAt)
            .UseValidHoldings(holdings)
            .Create()
            .ReportIsValid();
    }

    [Fact]
    public void ReportCreate_EmptyFilePath_IncorrectValidation()
    {
        var filePath = "";
        var createdAt = DateTime.Now;
        var holdings = new List<Holding>();

        new When()
            .UseInvalidFilePath(filePath)
            .UseValidCreatedAt(createdAt)
            .UseValidHoldings(holdings)
            .Create()
            .ReportIsNotValid();
    }

    [Fact]
    public void ReportCreate_NullFilePath_IncorrectValidation()
    {
        var createdAt = DateTime.Now;
        var holdings = new List<Holding>();

        new When()
            .UseValidCreatedAt(createdAt)
            .UseValidHoldings(holdings)
            .Create()
            .ReportIsNotValid();
    }

    [Fact]
    public void ReportCreate_NullHoldings_IncorrectValidation()
    {
        var filePath = "";
        var createdAt = DateTime.Now;

        new When()
            .UseInvalidFilePath(filePath)
            .UseValidCreatedAt(createdAt)
            .Create()
            .ReportIsNotValid();
    }

    private sealed class When
    {
        private string filePath;
        private DateTime createdAt;
        private IEnumerable<Holding> holdings;
        private ErrorOr<Report> result;

        public When()
        {

        }

        public When UseValidFilePath(string input)
        {
            Assert.Null(filePath);
            filePath = input;
            return this;
        }

        public When UseInvalidFilePath(string input)
        {
            Assert.Null(filePath);
            filePath = input;
            return this;
        }

        public When UseValidCreatedAt(DateTime input)
        {
            createdAt = input;
            return this;
        }

        public When UseValidHoldings(IEnumerable<Holding> input)
        {
            holdings = input;
            return this;
        }

        public When Create()
        {
            result = Report.Create(filePath, holdings, createdAt);
            return this;
        }

        public When ReportIsValid()
        {
            Assert.False(result.IsError);
            var report = result.Value;
            Assert.Equal(report.ReportPathValue.PathValue, filePath);
            Assert.Equal(report.Holdings, holdings);
            Assert.Equal(report.CreatedAt, createdAt);
            return this;
        }

        public When ReportIsNotValid()
        {
            Assert.True(result.IsError);
            return this;
        }
    }
}