using ErrorOr;
using StocksReportingLibrary.Domain.Email;

namespace Test.Unit.Domain;

public class EmailUnitTest
{
    [Fact]
    public void EmailCreate_ValidInput_CorrectValidation()
    {
        var email = "me@gmail.com";

        new When()
            .UseValidEmailValue(email)
            .Create()
            .EmailIsValid();
    }

    [Fact]
    public void EmailCreate_EmptyString_IncorrectValidation()
    {
        var email = "";

        new When()
            .UseValidEmailValue(email)
            .Create()
            .EmailIsNotValid();
    }

    [Fact]
    public void EmailCreate_NullString_IncorrectValidation()
    {
        new When()
            .Create()
            .EmailIsNotValid();
    }

    [Fact]
    public void EmailCreate_InputMissingAtSign_IncorrectValidation()
    {
        var email = "megmail.com";

        new When()
            .UseValidEmailValue(email)
            .Create()
            .EmailIsNotValid();
    }

    [Fact]
    public void EmailCreate_InputMissingUser_IncorrectValidation()
    {
        var email = "@gmail.com";

        new When()
            .UseValidEmailValue(email)
            .Create()
            .EmailIsNotValid();
    }

    private sealed class When
    {
        private string emailValue;
        private ErrorOr<Email> result;

        public When()
        {

        }

        public When UseValidEmailValue(string input)
        {
            Assert.Null(emailValue);
            emailValue = input;
            return this;
        }

        public When UseInvalidEmailValue(string input)
        {
            Assert.Null(emailValue);
            emailValue = input;
            return this;
        }

        public When Create()
        {
            result = Email.Create(emailValue);
            return this;
        }

        public When EmailIsValid()
        {
            Assert.False(result.IsError);
            Assert.Equal(result.Value.EmailValue.Value, emailValue);
            return this;
        }

        public When EmailIsNotValid()
        {
            Assert.True(result.IsError);
            return this;
        }
    }
}