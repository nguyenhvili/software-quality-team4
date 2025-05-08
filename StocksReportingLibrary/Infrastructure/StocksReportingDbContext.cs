using Microsoft.EntityFrameworkCore;
using System.Reflection;
using StocksReportingLibrary.Domain.Email;
using StocksReportingLibrary.Domain.Report;
using StocksReportingLibrary.Domain.Report.Holding;
using Microsoft.Extensions.Logging;

namespace StocksReportingLibrary.Infrastructure;

public class StocksReportingDbContext : DbContext
{
    private readonly ILogger<StocksReportingDbContext> _logger;

    public StocksReportingDbContext(DbContextOptions<StocksReportingDbContext> options, ILogger<StocksReportingDbContext> logger) : base(options)
    {
        _logger = logger;
    }

    public DbSet<Domain.Email.Email> Emails { get; set; } = null!;

    public DbSet<Report> Reports { get; set; } = null!;

    public DbSet<Holding> Holdings { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _logger.LogDebug("Configuring model creating.");
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        _logger.LogDebug("Model creating configured.");
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        _logger.LogInformation("Saving changes to the database.");
        try
        {
            var result = await base.SaveChangesAsync(cancellationToken);
            _logger.LogDebug("{ChangesSaved} changes saved to the database.", result);
            return result;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error saving changes to the database.");
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during SaveChangesAsync.", ex);
            throw;
        }
    }
}
