using System.Linq.Expressions;
using StocksReportingLibrary.Application.Common;

namespace StocksReportingLibrary.Application.Services.Persistence;

public interface IQueryObject<TAggregate> where TAggregate : class
{
    IQueryObject<TAggregate> Filter(Expression<Func<TAggregate, bool>> predicate);
    IQueryObject<TAggregate> OrderBy(Expression<Func<TAggregate, object>> selector, bool ascending = true);
    IQueryObject<TAggregate> Page(Paging paging);

    Task<IEnumerable<TAggregate>> ExecuteAsync();
}
