using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.OpenApi.Models;
using Quartz;
using RegistR.Attributes.Extensions;
using StocksReporting.Application.Services.Schedulling;
using StocksReporting.Infrastructure;
using System.Reflection;
using Wolverine;
using Wolverine.Http;

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

builder.Services.AddQuartz(q =>
{
    var jobKey = new JobKey("CreateReportJob");

    q.AddJob<CreateReportJob>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("CreateReportJobTrigger")
        .WithCronSchedule("0 0 * ? * *"));
});

builder.Services.AddQuartzHostedService();


var dbString = builder.Configuration.GetConnectionString("DbString");
builder.Services.InstallRegisterAttribute(Assembly.GetExecutingAssembly());
builder.Services.AddDbContext<StocksReportingDbContext>(options =>
{
    options.UseSqlite(dbString);
});

builder.Host.UseWolverine();

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
