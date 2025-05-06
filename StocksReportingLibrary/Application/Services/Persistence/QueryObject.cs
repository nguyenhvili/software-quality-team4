using System.Linq.Expressions;
using StocksReportingLibrary.Application.Common;

namespace StocksReportingLibrary.Application.Services.Persistence;

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
                    ? _query.OrderBy(criteria.selector)
                    : _query.OrderByDescending(criteria.selector);
            }
            else
            {
                orderedQuery = criteria.ascending
                    ? orderedQuery.ThenBy(criteria.selector)
                    : orderedQuery.ThenByDescending(criteria.selector);
            }
        }

        return orderedQuery!;
    }

    public abstract Task<IEnumerable<TAggregate>> ExecuteAsync();
}
