using Microsoft.EntityFrameworkCore;
using RegistR.Attributes.Extensions;
using StocksReporting.Infrastructure;
using System.Reflection;
using StocksReporting.Application.Common.Interfaces;
using StocksReporting.Infrastructure.Email;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(builder.Configuration);

var dbString = builder.Configuration.GetConnectionString("DbString");
builder.Services.InstallRegisterAttribute(Assembly.GetExecutingAssembly());
builder.Services.AddDbContext<StocksReportingDbContext>(options =>
{
    options.UseSqlite(dbString);
});

builder.Services.AddScoped<IEmailSender, SmtpEmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
