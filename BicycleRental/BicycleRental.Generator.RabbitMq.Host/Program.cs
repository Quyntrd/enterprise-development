using BicycleRental.Generator.RabbitMq.Host.Services;
using BicycleRental.ServiceDefaults;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.AddRabbitMQClient("rabbitmq",
    configureConnectionFactory: factory =>
    {
        factory.AutomaticRecoveryEnabled = true;
        factory.NetworkRecoveryInterval = TimeSpan.FromSeconds(5);
        factory.TopologyRecoveryEnabled = true;
        factory.RequestedHeartbeat = TimeSpan.FromSeconds(30);
        factory.DispatchConsumersAsync = true;
    });

builder.Services.AddScoped<IProducerService, BicycleRentalRabbitMqProducer>();

builder.Services.AddHostedService<BicycleModelGeneratorService>();
builder.Services.AddHostedService<BicycleGeneratorService>();
builder.Services.AddHostedService<RenterGeneratorService>();
builder.Services.AddHostedService<RentalGeneratorService>();

var app = builder.Build();

app.MapDefaultEndpoints();

app.Run();