var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddPostgres("postgres-16")
    .WithImageTag("16")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume()
    .AddDatabase("librarydb");

builder.AddProject<Projects.Library_Api>("library-api")
    .WithReference(db)
    .WaitFor(db);

builder.Build().Run();
