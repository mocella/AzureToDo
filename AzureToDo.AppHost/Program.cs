var builder = DistributedApplication.CreateBuilder(args);

var sqlpassword = builder.Configuration["sqlpassword"];

var sql = builder.AddSqlServerContainer("sql")
    .AddDatabase("sqldata");

var cache = builder.AddRedisContainer("cache");

var apiservice = builder.AddProject<Projects.AzureToDo_ApiService>("apiservice")
    .WithReference(sql);

builder.AddProject<Projects.AzureToDo_Web>("webfrontend")
    .WithReference(cache)
    .WithReference(apiservice);

builder.Build().Run();
