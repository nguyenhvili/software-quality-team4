using ErrorOr;
using StocksReportingLibrary.Domain.Email;
using StocksReportingLibrary.Domain.EmailAttachmentPath;

namespace Test.Unit.Domain;

public class EmailAttachmentPathUnitTest
{
    [Fact]
    public void EmailCreate_ValidInput_CorrectValidation()
    {
        var path = "2025-04-23_stocks_report.csv";

        new When()
            .UseValidAttachmentPath(path)
            .Create()
            .EmailAttachmentPathIsValid();
    }

    [Fact]
    public void EmailCreate_EmptyString_IncorrectValidation()
    {
        var path = "";

        new When()
            .UseInvalidAttachmentPath(path)
            .CreateInvalid();
    }

    [Fact]
    public void EmailCreate_NullString_IncorrectValidation()
    {
        new When()
            .CreateInvalid();
    }

    private sealed class When
    {
        private string path;
        private EmailAttachmentPath result;

        public When()
        {

        }

        public When UseValidAttachmentPath(string input)
        {
            Assert.Null(path);
            path = input;
            return this;
        }

        public When UseInvalidAttachmentPath(string input)
        {
            Assert.Null(path);
            path = input;
            return this;
        }

        public When Create()
        {
            result = EmailAttachmentPath.Create(path);
            return this;
        }

        public When EmailAttachmentPathIsValid()
        {
            Assert.Equal(result.Path, path);
            return this;
        }

        public When CreateInvalid()
        {
            Assert.Throws<ArgumentException>(() => EmailAttachmentPath.Create(path));
            return this;
        }
    }
}
