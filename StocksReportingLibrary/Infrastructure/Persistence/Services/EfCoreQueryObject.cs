using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RegistR.Attributes.Base;
using StocksReportingLibrary.Application.Services.Persistence;

namespace StocksReportingLibrary.Infrastructure.Persistence.Services;

[Register(ServiceLifetime.Transient, typeof(IQueryObject<>))]
public class EfCoreQueryObject<TAggregate> : QueryObject<TAggregate> where TAggregate : class
{
    private readonly StocksReportingDbContext _dbContext;

    public EfCoreQueryObject(StocksReportingDbContext dbContext)
    {
        _dbContext = dbContext;
        _query = _dbContext.Set<TAggregate>().AsQueryable();
    }

    public override async Task<IEnumerable<TAggregate>> ExecuteAsync()
    {
        return await _query.ToListAsync();
    }
}
