using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.OpenApi.Models;
using RegistR.Attributes.Extensions;
using StocksReporting.Infrastructure;
using System.Reflection;
using Wolverine;
using Wolverine.Http;
using StocksReporting.Application.Common.Interfaces;
using StocksReporting.Application.Report;
using StocksReporting.Application.Services;
using StocksReporting.Domain.Common;
using StocksReporting.Infrastructure.Email;
using StocksReporting.Infrastructure.Persistence.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(builder.Configuration);

builder.Services.AddSwaggerGen(c =>
{

    c.CustomSchemaIds(s => s.FullName.Replace("+", "."));
    c.SwaggerDoc("v1", new OpenApiInfo() { Title = "Stocks Reporting", Version = "v1" });
});

var dbString = builder.Configuration.GetConnectionString("DbString");
builder.Services.InstallRegisterAttribute(Assembly.GetExecutingAssembly());
builder.Services.AddDbContext<StocksReportingDbContext>(options =>
{
    options.UseSqlite(dbString);
});

builder.Host.UseWolverine();

builder.Services.AddScoped<ReportEmailService>();
builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfCoreRepository<>));


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<StocksReportingDbContext>();
    dbContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Stocks Reporting");


    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.MapWolverineEndpoints();

app.Run();
