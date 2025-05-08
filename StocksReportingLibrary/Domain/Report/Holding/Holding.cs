using ErrorOr;
using StocksReportingLibrary.Domain.Common;
using StocksReportingLibrary.Domain.Report.Holding.ValueObjects;

namespace StocksReportingLibrary.Domain.Report.Holding;

public class Holding : AggregateRoot<HoldingId>
{
    public Company Company { get; private set; }
    public Ticker Ticker {  get; private set; }
    public Shares Shares { get; private set; }
    public SharesPercent SharesPercent { get; private set; }
    public Weight Weight { get; private set; }

    public Holding()
    {

    }

    private Holding(HoldingId id, Company company, Ticker ticker, Shares shares, SharesPercent sharesPercent, Weight weight) : base(id)
    {
        Company = company;
        Ticker = ticker;
        Shares = shares;
        SharesPercent = sharesPercent;
        Weight = weight;
    }

    public static ErrorOr<Holding> Create(string company, string ticker, long shares, decimal sharesPercent, decimal weight)
    {
        if (company == null || company.Length == 0)
        {
            return Error.Validation("The company name is empty!");
        }
        if (shares < 0)
        {
            return Error.Validation("The shares cannot negative!");
        }
        if (weight < 0)
        {
            return Error.Validation("The weight cannot be negative!");
        }
        return new Holding(HoldingId.CreateUnique(), Company.Create(company), Ticker.Create(ticker), Shares.Create(shares), SharesPercent.Create(sharesPercent), Weight.Create(weight));
    }

    public void UpdateSharesPercent(SharesPercent sharesPercent)
    {
        this.SharesPercent = sharesPercent;
    }
}
