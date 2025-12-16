var builder = DistributedApplication.CreateBuilder(args);

var dbName = "BicycleRentalDatabase";

var bicycleDb = builder
    .AddMySql("bicycle-mysql")
    .AddDatabase(dbName);

var batchSize = builder.AddParameter("GeneratorBatchSize");
var payloadLimit = builder.AddParameter("GeneratorPayloadLimit");
var waitTime = builder.AddParameter("GeneratorWaitTime");

var rabbitUser = builder.AddParameter("RabbitMQLogin");
var rabbitPassword = builder.AddParameter("RabbitMQPassword");
var rabbitQueue = builder.AddParameter("RabbitMQQueue");

var rabbitMq = builder
    .AddRabbitMQ("rabbitmq", userName: rabbitUser, password: rabbitPassword)
    .WithManagementPlugin();

builder.AddProject<Projects.BicycleRental_Api>("bicyclerental-api-host")
    .WithReference(bicycleDb, "BicycleRentalDatabase")
    .WithReference(rabbitMq)
    .WithEnvironment("RabbitMq__QueueName", rabbitQueue)
    .WaitFor(bicycleDb)
    .WaitFor(rabbitMq);

builder.AddProject<Projects.BicycleRental_Generator_RabbitMq_Host>("bicyclerental-generator-rabbitmq-host")
    .WithReference(rabbitMq)
    .WithEnvironment("RabbitMq__QueueName", rabbitQueue)
    .WithEnvironment("Generator:BatchSize", batchSize)
    .WithEnvironment("Generator:PayloadLimit", payloadLimit)
    .WithEnvironment("Generator:WaitTime", waitTime)
    .WaitFor(rabbitMq);

builder.Build().Run();
