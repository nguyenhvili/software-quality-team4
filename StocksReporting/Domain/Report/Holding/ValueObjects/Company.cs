using StocksReporting.Domain.Common;

namespace StocksReporting.Domain.Report.Holding.ValueObjects;

public class Company : ValueObject
{
    public string Name { get; set; }

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
