using ErrorOr;
using StocksReportingLibrary.Domain.Report.Holding;
using StocksReportingLibrary.Domain.Report.Holding.ValueObjects;

namespace Test.Unit.Domain;

public class HoldingUnitTest
{
    [Fact]
    public void HoldingCreate_ValidInput_CorrectValidation()
    {
        var company = "Tesla";
        var ticker = "TSL";
        var shares = 5000L;
        decimal sharesPercent = 5.5M;
        decimal weight = 6.5M;

        new When()
            .UseValidCompany(company)
            .UseValidTicker(ticker)
            .UseValidShares(shares)
            .UseValidSharesPercent(sharesPercent)
            .UseValidWeight(weight)
            .Create()
            .HoldingIsValid();
    }

    [Fact]
    public void HoldingCreate_EmptyCompany_IncorrectValidation()
    {
        var company = "";
        var ticker = "TSL";
        var shares = 5000L;
        decimal sharesPercent = 5.5M;
        decimal weight = 6.5M;

        new When()
            .UseInvalidCompany(company)
            .UseValidTicker(ticker)
            .UseValidShares(shares)
            .UseValidSharesPercent(sharesPercent)
            .UseValidWeight(weight)
            .Create()
            .HoldingIsNotValid();
    }

    [Fact]
    public void HoldingCreate_NullCompany_IncorrectValidation()
    {
        var ticker = "TSL";
        var shares = 5000L;
        decimal sharesPercent = 5.5M;
        decimal weight = 6.5M;

        new When()
            .UseValidTicker(ticker)
            .UseValidShares(shares)
            .UseValidSharesPercent(sharesPercent)
            .UseValidWeight(weight)
            .Create()
            .HoldingIsNotValid();
    }

    [Fact]
    public void HoldingCreate_NegativeShares_IncorrectValidation()
    {
        var company = "Tesla";
        var ticker = "TSL";
        var shares = -5000L;
        decimal sharesPercent = 5.5M;
        decimal weight = 6.5M;

        new When()
            .UseValidCompany(company)
            .UseValidTicker(ticker)
            .UseInvalidShares(shares)
            .UseValidSharesPercent(sharesPercent)
            .UseValidWeight(weight)
            .Create()
            .HoldingIsNotValid();
    }

    [Fact]
    public void HoldingCreate_NegativeWeight_IncorrectValidation()
    {
        var company = "Tesla";
        var ticker = "TSL";
        var shares = 5000L;
        decimal sharesPercent = 5.5M;
        decimal weight = -6.5M;

        new When()
            .UseValidCompany(company)
            .UseValidTicker(ticker)
            .UseInvalidShares(shares)
            .UseValidSharesPercent(sharesPercent)
            .UseValidWeight(weight)
            .Create()
            .HoldingIsNotValid();
    }

    [Fact]
    public void HoldingUpdateSharesPercent_ValidInput_SharesUpdated()
    {
        var company = "Tesla";
        var ticker = "TSL";
        var shares = 5000L;
        decimal sharesPercent = 5.5M;
        decimal weight = 6.5M;
        Holding holding = (Holding.Create(company, ticker, shares, sharesPercent, weight)).Value;
        SharesPercent newSharesPercent = SharesPercent.Create(6.9M);

        new When()
            .UseValidHolding(holding)
            .UseValidNewSharesPercent(newSharesPercent)
            .UpdateSharesPercent()
            .HoldingSharesPercentUpdated();
    }

    private sealed class When
    {
        private string company;
        private string ticker;
        private long shares;
        private decimal sharesPercent;
        private decimal weight;
        private SharesPercent newSharesPercent;
        private ErrorOr<Holding> result;
        private Holding holding;

        public When()
        {

        }

        public When UseValidCompany(string input)
        {
            Assert.Null(company);
            company = input;
            return this;
        }

        public When UseInvalidCompany(string input)
        {
            Assert.Null(company);
            company = input;
            return this;
        }

        public When UseValidTicker(string input)
        {
            Assert.Null(ticker);
            ticker = input;
            return this;
        }

        public When UseValidShares(long input)
        {
            shares = input;
            return this;
        }

        public When UseInvalidShares(long input)
        {
            shares = input;
            return this;
        }

        public When UseValidSharesPercent(decimal input)
        {
            sharesPercent = input;
            return this;
        }

        public When UseValidWeight(decimal input)
        {
            weight = input;
            return this;
        }

        public When UseInvalidWeight(decimal input)
        {
            weight = input;
            return this;
        }

        public When UseValidHolding(Holding input)
        {
            Assert.Null(holding);
            holding = input;
            return this;
        }

        public When UseValidNewSharesPercent(SharesPercent input)
        {
            Assert.Null(newSharesPercent);
            newSharesPercent = input;
            return this;
        }

        public When Create()
        {
            result = Holding.Create(company, ticker, shares, sharesPercent, weight);
            return this;
        }

        public When UpdateSharesPercent()
        {
            Assert.NotNull(newSharesPercent);
            Assert.NotNull(holding);
            holding.UpdateSharesPercent(newSharesPercent);
            return this;
        }

        public When HoldingIsValid()
        {
            Assert.False(result.IsError);
            var holding = result.Value;
            Assert.Equal(holding.Company.Name, company);
            Assert.Equal(holding.Ticker.Value, ticker);
            Assert.Equal(holding.Shares.Value, shares);
            Assert.Equal(holding.SharesPercent.Value, sharesPercent);
            Assert.Equal(holding.Weight.Value, weight);
            return this;
        }

        public When HoldingIsNotValid()
        {
            Assert.True(result.IsError);
            return this;
        }

        public When HoldingSharesPercentUpdated()
        {
            Assert.Equal(holding.SharesPercent, newSharesPercent);
            return this;
        }
    }
}
