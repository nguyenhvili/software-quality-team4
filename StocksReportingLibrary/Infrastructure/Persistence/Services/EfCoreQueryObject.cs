using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RegistR.Attributes.Base;
using StocksReportingLibrary.Application.Services.Persistence;
using Microsoft.Extensions.Logging;

namespace StocksReportingLibrary.Infrastructure.Persistence.Services;

[Register(ServiceLifetime.Transient, typeof(IQueryObject<>))]
public class EfCoreQueryObject<TAggregate> : QueryObject<TAggregate> where TAggregate : class
{
    private readonly StocksReportingDbContext _dbContext;
    private readonly ILogger<EfCoreQueryObject<TAggregate>> _logger;

    public EfCoreQueryObject(
        StocksReportingDbContext dbContext,
        ILogger<EfCoreQueryObject<TAggregate>> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
        _query = _dbContext.Set<TAggregate>().AsQueryable();
    }

    public override async Task<IEnumerable<TAggregate>> ExecuteAsync()
    {
        _logger.LogInformation("Executing query to retrieve all {AggregateType}", typeof(TAggregate).Name);
        try
        {
            var result = await _query.ToListAsync();
            _logger.LogDebug("Query executed successfully. Retrieved {ResultCount} {AggregateType}s", result.Count, typeof(TAggregate).Name);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing query to retrieve all {AggregateType}", typeof(TAggregate).Name);
            throw;
        }
    }
}
