using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Quartz;
using RegistR.Attributes.Extensions;
using StocksReporting.Infrastructure;
using System.Reflection;
using Wolverine;
using Wolverine.Http;
using StocksReporting.Application.Report;
using StocksReporting.Application.Services;
using StocksReporting.Application.Services.Scheduling;
using StocksReporting.Infrastructure.Email;
using StocksReporting.Infrastructure.Persistence.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost3000",
        policy => policy.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

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

    // For testing purpose and for M2 showcase, the trigger is set to start at the start of the program
    // The commented Cron schedule is set to trigger the trigger at midnight, first day of a month
    // at 0 seconds, 0 minutes, 0 hours, 1st day of month, * every month, ? - any day of the week
    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("CreateReportJobTrigger")
        .StartNow());
        //.WithCronSchedule("0 0 0 1 * ?"));
});

builder.Services.AddQuartzHostedService();


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

app.UseCors("AllowLocalhost3000");

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

app.MapFallbackToFile("/index.html");

app.Run();
