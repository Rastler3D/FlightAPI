var builder = DistributedApplication.CreateBuilder(args);

var database = builder.AddPostgres("database")
    .WithPgAdmin()
    .AddDatabase("flight-db");

var cache = builder.AddRedis("cache");

var apiService = builder.AddProject<Projects.Flight_Api>("flight-api")
    .WithExternalHttpEndpoints()
    .WithHttpHealthCheck("/health")
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(database)
    .WaitFor(database);


builder.Build().Run();