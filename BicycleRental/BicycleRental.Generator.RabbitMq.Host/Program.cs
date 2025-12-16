using BicycleRental.Application.Contracts.BicycleModels;
using BicycleRental.Application.Contracts.Bicycles;
using BicycleRental.Application.Contracts.Rentals;
using BicycleRental.Application.Contracts.Renters;
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

builder.Services.AddSingleton<IGeneratorStrategy<BicycleCreateUpdateDto>, BicycleGeneratorStrategy>();
builder.Services.AddSingleton<IGeneratorStrategy<BicycleModelCreateUpdateDto>, BicycleModelGeneratorStrategy>();
builder.Services.AddSingleton<IGeneratorStrategy<RenterCreateUpdateDto>, RenterGeneratorStrategy>();
builder.Services.AddSingleton<IGeneratorStrategy<RentalCreateUpdateDto>, RentalGeneratorStrategy>();

builder.Services.AddHostedService<GeneratorServiceBase<BicycleCreateUpdateDto>>();
builder.Services.AddHostedService<GeneratorServiceBase<BicycleModelCreateUpdateDto>>();
builder.Services.AddHostedService<GeneratorServiceBase<RenterCreateUpdateDto>>();
builder.Services.AddHostedService<GeneratorServiceBase<RentalCreateUpdateDto>>();


var app = builder.Build();

app.MapDefaultEndpoints();

app.Run();