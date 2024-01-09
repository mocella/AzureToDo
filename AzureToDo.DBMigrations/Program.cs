using AzureToDo.Db.Entities;
using AzureToDo.DBMigrations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddNpgsqlDbContext<TicketContext>("ticketdb", null,
    optionsBuilder => optionsBuilder.UseNpgsql(npgsqlBuilder =>
        npgsqlBuilder.MigrationsAssembly(typeof(Program).Assembly.GetName().Name)));

builder.Services.AddOpenTelemetry()
    .WithTracing(tracing => tracing.AddSource(TicketDbInitializer.ActivitySourceName));

builder.Services.AddSingleton<TicketDbInitializer>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<TicketDbInitializer>());
builder.Services.AddHealthChecks()
    .AddCheck<TicketDbInitializerHealthCheck>("DbInitializer", null);

var app = builder.Build();

app.MapDefaultEndpoints();

app.Run();
