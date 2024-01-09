using System.Diagnostics;
using AzureToDo.DBMigrations.Migrations;
using FluentMigrator.Runner;
using Microsoft.Extensions.Options;

namespace AzureToDo.DBMigrations
{
    internal class TicketDbInitializer : BackgroundService
    {
        public const string ActivitySourceName = "Migrations";

        private readonly ActivitySource _activitySource = new(ActivitySourceName);
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ConnectionStringConfig _connectionStringConfig;

        public TicketDbInitializer(IServiceScopeFactory serviceScopeFactory,
            IOptions<ConnectionStringConfig> connectionStringConfig)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _connectionStringConfig = connectionStringConfig.Value;
        }

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using var activity = _activitySource.StartActivity("Initializing ticket database", ActivityKind.Client);
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var migrationRunner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                MigrationExtensions.EnsureDataBaseExists(_connectionStringConfig.TicketDB, "ticketdb");
                
                migrationRunner.MigrateUp();
            }
            
            return Task.CompletedTask;
        }
    }
}
