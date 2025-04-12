using Microsoft.EntityFrameworkCore;
using RegistR.Attributes.Base;
using StocksReporting.Application.Services;

namespace StocksReporting.Infrastructure.Persistance.Services;

[Register(ServiceLifetime.Transient, typeof(IQueryObject<>))]
public class EfCoreQueryObject<TAggregate> : QueryObject<TAggregate> where TAggregate : class
{
    private readonly DepartureCustomerServicesDbContext _dbContext;

    public EfCoreQueryObject(DepartureCustomerServicesDbContext dbContext)
    {
        _dbContext = dbContext;
        _query = _dbContext.Set<TAggregate>().AsQueryable();
    }

    public override async Task<IEnumerable<TAggregate>> ExecuteAsync()
    {
        return await _query.ToListAsync();
    }
}
