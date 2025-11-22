var builder = DistributedApplication.CreateBuilder(args);

var dbName = "bicycle_rental";

var bicycleDb = builder
    .AddMySql("bicycle-mysql")
    .AddDatabase(dbName);

builder.AddProject<Projects.BicycleRental_Api>("bicyclerental-api-host")
    .WithReference(bicycleDb, "Database")
    .WaitFor(bicycleDb);

builder.Build().Run();