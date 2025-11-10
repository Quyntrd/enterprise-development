using System.Reflection;
using AutoMapper;
using BicycleRental.Application.Mapping;
using BicycleRental.Infrastructure.InMemory;
using BicycleRental.Domain; // IRepository<T,K>
using BicycleRental.Domain.Models; // BicycleModel, Bicycle, Renter, Rental
using BicycleRental.Api.Contracts.Contracts;
using BicycleRental.Application.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// === AutoMapper: explicit MapperConfiguration to avoid ambiguous overloads ===
var mapperConfig = new MapperConfiguration(cfg =>
    cfg.AddProfile(new BicycleRentalProfile()), LoggerFactory.Create(builder => builder.AddConsole())
);
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

// === Repositories: ensure repository implementations implement BicycleRental.Domain.IRepository<,> ===
builder.Services.AddSingleton<IRepository<BicycleModel, int>, BicycleModelInMemoryRepository>();
builder.Services.AddSingleton<IRepository<Bicycle, int>, BicycleInMemoryRepository>();
builder.Services.AddSingleton<IRepository<Renter, int>, RenterInMemoryRepository>();
builder.Services.AddSingleton<IRepository<Rental, int>, RentalInMemoryRepository>();

// === Application services (interfaces come from Contracts, implementations from BicycleRental.Application) ===
builder.Services.AddScoped<BicycleRental.Api.Contracts.Contracts.IBicycleModelService, BicycleModelService>();
builder.Services.AddScoped<BicycleRental.Api.Contracts.Contracts.IBicycleService, BicycleService>();
builder.Services.AddScoped<BicycleRental.Api.Contracts.Contracts.IRenterService, RenterService>();
builder.Services.AddScoped<BicycleRental.Api.Contracts.Contracts.IRentalService, RentalService>();

// Controllers + Swagger
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
