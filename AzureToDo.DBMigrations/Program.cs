using AzureToDo.Db.Entities;
using AzureToDo.DBMigrations;
using FluentMigrator.Runner;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<TicketContext>("ticketdb", null,
    optionsBuilder => optionsBuilder.UseNpgsql(npgsqlBuilder =>
        npgsqlBuilder.MigrationsAssembly(typeof(Program).Assembly.GetName().Name)));

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(TicketDbInitializer.ActivitySourceName));

builder.Services.Configure<ConnectionStringConfig>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.AddFluentMigratorCore()
        .ConfigureRunner(rb => rb
               .AddPostgres()
                      .WithGlobalConnectionString(builder.Configuration.GetConnectionString("ticketdb"))
                             .ScanIn(typeof(Program).Assembly).For.Migrations())
    .AddLogging(lb => lb.AddFluentMigratorConsole());

builder.Services.AddScoped<TicketDbInitializer>();
builder.Services.AddHostedService(sp =>
{
    using var scope = sp.CreateScope();
    return scope.ServiceProvider.GetRequiredService<TicketDbInitializer>();
});

builder.Services.AddHealthChecks()
    .AddCheck<TicketDbInitializerHealthCheck>("DbInitializer", null);

var app = builder.Build();

app.MapDefaultEndpoints();

app.Run();
