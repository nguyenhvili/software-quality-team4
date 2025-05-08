using ErrorOr;

namespace StocksReportingLibrary.Application.Common;

public class Paging
{
    private const int MaxPageSize = 100;
    public int Page { get; }
    public int PageSize { get; }

    private Paging(int page, int pageSize)
    {
        Page     = page;
        PageSize = pageSize;
    }

    public static ErrorOr<Paging> Create(int page, int pageSize)
    {
        if (page < 1)
        {
            return Error.Validation("Page must be greater than 0");
        }

        if (pageSize is < 1 or > MaxPageSize)
        {
            return Error.Validation($"Page size must be between 1 and {MaxPageSize}");
        }

        return new Paging(page, pageSize);
    }

    public int Skip() => (Page - 1) * PageSize;
    public int Take() => PageSize;

    protected IEnumerable<object?> GetEqualityComponents()
    {
        yield return Page;
        yield return PageSize;
    }
}
