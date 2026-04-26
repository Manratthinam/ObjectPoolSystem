using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using DotNetEnv;
using ObjectPoolSystem.Domain.Pools;
using ObjectPoolSystem.Infrastructure.Resources;
using ObjectPoolSystem.Application.Interface;
using ObjectPoolSystem.Application.Services;
using ObjectPoolSystem.WebApi.Middleware;
using ObjectPoolSystem.Domain.Interfaces;
using ObjectPoolSystem.Infrastructure.Repositories;

Env.TraversePath().Load();

var builder = WebApplication.CreateBuilder(args);

var dbHost     = Environment.GetEnvironmentVariable("DB_HOST")     ?? "localhost";
var dbPort     = Environment.GetEnvironmentVariable("DB_PORT")     ?? "5432";
var dbName     = Environment.GetEnvironmentVariable("DB_NAME")     ?? "postgres";
var dbUsername = Environment.GetEnvironmentVariable("DB_USERNAME") ?? throw new InvalidOperationException("DB_USERNAME is not set in .env");
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? throw new InvalidOperationException("DB_PASSWORD is not set in .env");

var connectionString =
    $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUsername};Password={dbPassword};Pooling=false";

var emailUsername = Environment.GetEnvironmentVariable("EMAIL_USERNAME")
    ?? throw new InvalidOperationException("EMAIL_USERNAME is not set in .env");

var emailPassword = Environment.GetEnvironmentVariable("EMAIL_PASSWORD")
    ?? throw new InvalidOperationException("EMAIL_PASSWORD is not set in .env");

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
    .MinimumLevel.Override("System", Serilog.Events.LogEventLevel.Warning)
    .WriteTo.Console()
    .WriteTo.File("app_logs.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ObjectPool<IPoolDbConnection>>(sp =>
    new ObjectPool<IPoolDbConnection>(
        () => new PoolDbContext(connectionString),
        2, 5, new ObjectPoolSystem.Domain.Models.PoolStatistics()
    ));

builder.Services.AddSingleton<ObjectPool<IPoolSmtpConnection>>(sp =>
    new ObjectPool<IPoolSmtpConnection>(
        () => new PoolSmtpClient("smtp.gmail.com", 587, emailUsername, emailPassword),
        2, 5, new ObjectPoolSystem.Domain.Models.PoolStatistics()
    ));

builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IDatabaseService, DatabaseService>();
builder.Services.AddTransient<IEmailService, ObjectPoolSystem.Application.Services.EmailService>();
builder.Services.AddHostedService<PoolMonitorService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var poolContext = new PoolDbContext(connectionString))
{
    poolContext.EnsureCreated();
}

app.Run();
