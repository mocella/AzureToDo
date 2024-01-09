using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using AzureToDo.Db.Entities;
using System;

namespace AzureToDo.DBMigrations
{
    internal class TicketDbInitializer(IServiceProvider serviceProvider, ILogger<TicketDbInitializer> logger)
        : BackgroundService
    {
        public const string ActivitySourceName = "Migrations";

        private readonly ActivitySource _activitySource = new(ActivitySourceName);

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TicketContext>();

            await InitializeDatabaseAsync(dbContext, cancellationToken);
        }

        private async Task InitializeDatabaseAsync(TicketContext ticketContext, CancellationToken cancellationToken)
        {
            using var activity = _activitySource.StartActivity("Initializing ticket database", ActivityKind.Client);

            var sw = Stopwatch.StartNew();

            await ticketContext.Database.EnsureCreatedAsync(cancellationToken);
            var strategy = ticketContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(ticketContext.Database.MigrateAsync, cancellationToken);

            logger.LogInformation("Database initialization completed after {ElapsedMilliseconds}ms", sw.ElapsedMilliseconds);
        }
    }
}
