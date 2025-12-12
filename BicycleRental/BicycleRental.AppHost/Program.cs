var builder = DistributedApplication.CreateBuilder(args);

var dbName = "bicycle-rental";

var bicycleDb = builder
    .AddMySql("bicycle-mysql")
    .AddDatabase(dbName);

builder.AddProject<Projects.BicycleRental_Api>("bicyclerental-api-host")
    .WithReference(bicycleDb, "BicycleRentalDatabase")
    .WaitFor(bicycleDb);

builder.Build().Run();