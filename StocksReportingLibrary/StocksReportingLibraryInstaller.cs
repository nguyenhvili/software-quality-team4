using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RegistR.Attributes.Extensions;
using StocksReportingLibrary.Infrastructure;
using System.Reflection;
using Wolverine.Attributes;

[assembly: WolverineModule]
namespace StocksReportingLibrary;

public static class StocksReportingLibraryInstaller
{
    public static IServiceCollection InstallStocksReportingLibrary(this IServiceCollection services, string dbString)
    {
        services.InstallRegisterAttribute(Assembly.GetExecutingAssembly());

        services.AddDbContext<StocksReportingDbContext>(options =>
        {
            options.UseSqlite(dbString);
        });

        return services;
    }
}
