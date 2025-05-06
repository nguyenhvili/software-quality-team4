using StocksReportingLibrary.Domain.Common;

namespace StocksReportingLibrary.Domain.Report.Holding.ValueObjects;

public class Company : ValueObject
{
    public string Name { get; private set; }

    private Company(string name)
    {
        this.Name = name;
    }

    public static Company Create(string name) => new(name);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Name;
    }
}
