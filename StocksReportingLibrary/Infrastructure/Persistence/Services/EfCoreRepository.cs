using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RegistR.Attributes.Base;
using StocksReportingLibrary.Application.Services.Persistence;
using Microsoft.Extensions.Logging;

namespace StocksReportingLibrary.Infrastructure.Persistence.Services;

[Register(ServiceLifetime.Transient, typeof(IRepository<>))]
public class EfCoreRepository<TAggregate> : IRepository<TAggregate> where TAggregate : class
{
    protected readonly StocksReportingDbContext _context;
    protected readonly DbSet<TAggregate> _dbSet;
    private readonly ILogger<EfCoreRepository<TAggregate>> _logger;

    public EfCoreRepository(
        StocksReportingDbContext context,
        ILogger<EfCoreRepository<TAggregate>> logger)
    {
        _context = context;
        _dbSet = _context.Set<TAggregate>();
        _logger = logger;
    }

    public async Task<TAggregate> InsertAsync(TAggregate entity)
    {
        _logger.LogInformation("Inserting new {AggregateType}", typeof(TAggregate).Name);
        await _dbSet.AddAsync(entity);
        _logger.LogDebug("Entity inserted.");
        return entity;
    }

    public async Task InsertRangeAsync(List<TAggregate> entities)
    {
        _logger.LogInformation("Inserting {EntityCount} {AggregateType}s", entities.Count, typeof(TAggregate).Name);
        await _dbSet.AddRangeAsync(entities);
        _logger.LogDebug("Entities inserted.");
    }

    public void Update(TAggregate entity)
    {
        _logger.LogInformation("Updating {AggregateType} with ID: {Entity}", typeof(TAggregate).Name, entity);
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        _logger.LogDebug("Entity updated.");
    }

    public async Task<bool> RemoveAsync(object id)
    {
        _logger.LogInformation("Removing {AggregateType} with ID: {EntityId}", typeof(TAggregate).Name, id);
        var entity = await _dbSet.FindAsync(id);
        if (entity is null)
        {
            _logger.LogWarning("{AggregateType} with ID: {EntityId} not found for removal.", typeof(TAggregate).Name, id);
            return false;
        }

        _dbSet.Remove(entity);
        _logger.LogDebug("Entity removed.");
        return true;
    }

    public async Task CommitAsync()
    {
        _logger.LogInformation("Committing changes to the database.");
        await _context.SaveChangesAsync();
        _logger.LogDebug("Changes committed.");
    }

    public async Task<TAggregate?> GetByIdAsync(Guid id)
    {
        _logger.LogInformation("Getting {AggregateType} by ID: {EntityId}", typeof(TAggregate).Name, id);
        var entity = await _dbSet.FindAsync(id);
        if (entity is null)
        {
            _logger.LogWarning("{AggregateType} with ID: {EntityId} not found.", typeof(TAggregate).Name, id);
        }
        else
        {
            _logger.LogDebug("Entity found.");
        }
        return entity;
    }
}
