using AutoMapper;
using BicycleRental.Application.Contracts.Contracts;
using BicycleRental.Application.Mapping;
using BicycleRental.Application.Services;
using BicycleRental.Domain;
using BicycleRental.Domain.Models;
using BicycleRental.Infrastructure.InMemory;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var mapperConfig = new MapperConfiguration(cfg =>
    cfg.AddProfile(new BicycleRentalProfile()), LoggerFactory.Create(builder => builder.AddConsole())
);
IMapper? mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddSingleton<IRepository<BicycleModel, int>, BicycleModelInMemoryRepository>();
builder.Services.AddSingleton<IRepository<Bicycle, int>, BicycleInMemoryRepository>();
builder.Services.AddSingleton<IRepository<Renter, int>, RenterInMemoryRepository>();
builder.Services.AddSingleton<IRepository<Rental, int>, RentalInMemoryRepository>();

builder.Services.AddScoped<IBicycleModelService, BicycleModelService>();
builder.Services.AddScoped<IBicycleService, BicycleService>();
builder.Services.AddScoped<IRenterService, RenterService>();
builder.Services.AddScoped<IRentalService, RentalService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BicycleRental API", Version = "v1" });

    var assembly = Assembly.GetExecutingAssembly();
    var xml = Path.Combine(AppContext.BaseDirectory, $"{assembly.GetName().Name}.xml");
    if (File.Exists(xml)) c.IncludeXmlComments(xml);

    foreach (var refAsm in assembly.GetReferencedAssemblies())
    {
        var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{refAsm.Name}.xml");
        if (File.Exists(xmlPath)) c.IncludeXmlComments(xmlPath);
    }
    c.SchemaFilter<BicycleRental.Api.Swagger.TimeSpanSchemaFilter>();
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();