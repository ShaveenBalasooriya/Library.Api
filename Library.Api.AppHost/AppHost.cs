var builder = DistributedApplication.CreateBuilder(args);

var db = builder.AddPostgres("postgres")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume()
    .AddDatabase("librarydb");

builder.AddProject<Projects.Library_Api>("library-api")
    .WithReference(db)
    .WaitFor(db);

builder.Build().Run();
