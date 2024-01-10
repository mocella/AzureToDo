var builder = DistributedApplication.CreateBuilder(args);

var postgresqlDb = builder.AddPostgresContainer("pg")
    .AddDatabase("ticketdb");

var cache = builder.AddRedisContainer("cache");

var apiService = builder.AddProject<Projects.AzureToDo_ApiService>("apiservice")
    .WithReference(postgresqlDb);

builder.AddProject<Projects.AzureToDo_Web>("webfrontend")
    .WithReference(cache)
    .WithReference(apiService);

builder.AddProject<Projects.AzureToDo_DBMigrations>("dbmigrations")
    .WithReference(postgresqlDb);

builder.Build().Run();
