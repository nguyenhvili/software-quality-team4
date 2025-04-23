using System.Linq.Expressions;
using StocksReporting.Application.Common;

namespace StocksReporting.Application.Services;

public interface IQueryObject<TAggregate> where TAggregate : class
{
    IQueryObject<TAggregate> Filter(Expression<Func<TAggregate, bool>> predicate);
    IQueryObject<TAggregate> OrderBy(Expression<Func<TAggregate, object>> selector, bool ascending = true);
    IQueryObject<TAggregate> Page(Paging paging);

    Task<IEnumerable<TAggregate>> ExecuteAsync();
}
