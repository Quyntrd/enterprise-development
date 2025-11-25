using BicycleRental.Application.Contracts.Contracts;
using BicycleRental.Application.Mapping;
using BicycleRental.Application.Services;
using BicycleRental.Domain;
using BicycleRental.Domain.Models;
using BicycleRental.Infrastructure.EfCore;
using BicycleRental.Infrastructure.EfCore.Repositories;
using BicycleRental.ServiceDefaults;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddAutoMapper(config =>
{
    config.AddProfile(new BicycleRentalProfile());
});

builder.Services.AddScoped<IRepository<BicycleModel, int>, BicycleModelEfCoreRepository>();
builder.Services.AddScoped<IRepository<Bicycle, int>, BicycleEfCoreRepository>();
builder.Services.AddScoped<IRepository<Renter, int>, RenterEfCoreRepository>();
builder.Services.AddScoped<IRepository<Rental, int>, RentalEfCoreRepository>();

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

var conn = builder.Configuration.GetConnectionString("BicycleRentalDatabase");

builder.Services.AddDbContext<BicycleRentalDbContext>(options =>
    options.UseMySql(conn, ServerVersion.AutoDetect(conn)));

var app = builder.Build();

app.MapDefaultEndpoints();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<BicycleRentalDbContext>();
    db.Database.Migrate();

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
