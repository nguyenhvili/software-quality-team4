using System.Linq.Expressions;
using StocksReporting.Application.Common;

namespace StocksReporting.Application.Services;

public abstract class QueryObject<TAggregate> : IQueryObject<TAggregate> where TAggregate : class
{
    protected IQueryable<TAggregate> _query;
    protected List<(Expression<Func<TAggregate, object>> selector, bool ascending)> _sortingCriteria = [];

    public IQueryObject<TAggregate> Filter(Expression<Func<TAggregate, bool>> predicate)
    {
        _query = _query.Where(predicate);
        return this;
    }

    public IQueryObject<TAggregate> Page(Paging paging)
    {
        _query = _query.Skip(paging.Skip()).Take(paging.Take());
        return this;
    }

    public IQueryObject<TAggregate> OrderBy(Expression<Func<TAggregate, object>> selector, bool ascending = true)
    {
        _sortingCriteria.Add((selector, ascending));
        _query = ApplySorting();
        return this;
    }

    protected IQueryable<TAggregate> ApplySorting()
    {
        if (_sortingCriteria.Count == 0)
            return _query;

        IOrderedQueryable<TAggregate> orderedQuery = null;

        foreach (var criteria in _sortingCriteria)
        {
            if (orderedQuery == null)
            {
                orderedQuery = criteria.ascending
                    ? Queryable.OrderBy(_query, criteria.selector)
                    : Queryable.OrderByDescending(_query, criteria.selector);
            }
            else
            {
                orderedQuery = criteria.ascending
                    ? Queryable.ThenBy(orderedQuery, criteria.selector)
                    : Queryable.ThenByDescending(orderedQuery, criteria.selector);
            }
        }

        return orderedQuery!;
    }

    public abstract Task<IEnumerable<TAggregate>> ExecuteAsync();
}
