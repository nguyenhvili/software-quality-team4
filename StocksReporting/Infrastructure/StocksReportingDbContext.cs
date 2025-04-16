using Microsoft.EntityFrameworkCore;
using System.Reflection;
using StocksReporting.Domain.Email;
using StocksReporting.Domain.Report;
using StocksReporting.Domain.Report.Holding;

namespace StocksReporting.Infrastructure;

public class StocksReportingDbContext : DbContext
{
    public StocksReportingDbContext() { }

    public StocksReportingDbContext(DbContextOptions<StocksReportingDbContext> options) : base(options)
    {
        
    }

    public DbSet<Email> Emails { get; set; } = null!;
    
    public DbSet<Report> Reports { get; set; } = null!;

    public DbSet<Holding> Holdings { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }
}
