using BicycleRental.Domain.DataSeed;
using BicycleRental.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace BicycleRental.Infrastructure.EfCore;

/// <summary>
/// EF Core DbContext for BicycleRental domain.
/// Contains DbSet for BicycleModel, Bicycle, Renter and Rental.
/// Seeds initial data from <see cref="BicycleRentalDataSeed"/> via HasData in OnModelCreating.
/// </summary>
public class BicycleRentalDbContext(DbContextOptions<BicycleRentalDbContext> options) : DbContext(options)
{
    public DbSet<BicycleModel> BicycleModels { get; set; }
    public DbSet<Bicycle> Bicycles { get; set; }
    public DbSet<Renter> Renters { get; set; }
    public DbSet<Rental> Rentals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BicycleModel>(b =>
        {
            b.ToTable("bicycle_models");
            b.HasKey(x => x.Id);
            b.Property(x => x.Name).IsRequired().HasMaxLength(200).HasColumnName("name");
            b.Property(x => x.Type).IsRequired().HasColumnName("type");
            b.Property(x => x.WheelSizeInInches).HasColumnName("wheel_size_in_inches");
            b.Property(x => x.MaxPassengerWeightKg).HasColumnName("max_passenger_weight_kg");
            b.Property(x => x.WeightKg).HasColumnName("weight_kg");
            b.Property(x => x.BrakeType).HasMaxLength(100).HasColumnName("brake_type");
            b.Property(x => x.ModelYear).HasColumnName("model_year");
            b.Property(x => x.PricePerHour).HasColumnType("decimal(10,2)").HasColumnName("price_per_hour");
        });

        modelBuilder.Entity<Bicycle>(b =>
        {
            b.ToTable("bicycles");
            b.HasKey(x => x.Id);
            b.Property(x => x.SerialNumber).IsRequired().HasMaxLength(100).HasColumnName("serial_number");
            b.Property(x => x.ModelId).IsRequired().HasColumnName("model_id");
            b.Property(x => x.Color).HasMaxLength(50).HasColumnName("color");

            b.HasOne<BicycleModel>()
             .WithMany()
             .HasForeignKey(x => x.ModelId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Renter>(b =>
        {
            b.ToTable("renters");
            b.HasKey(x => x.Id);
            b.Property(x => x.FirstName).IsRequired().HasMaxLength(30).HasColumnName("first_name");
            b.Property(x => x.LastName).IsRequired().HasMaxLength(30).HasColumnName("last_name");
            b.Property(x => x.Patronymic).HasMaxLength(30).HasColumnName("patronymic");
            b.Property(x => x.Phone).IsRequired().HasMaxLength(20).HasColumnName("phone");
        });

        modelBuilder.Entity<Rental>(b =>
        {
            b.ToTable("rentals");
            b.HasKey(x => x.Id);
            b.Property(x => x.BicycleId).IsRequired().HasColumnName("bicycle_id");
            b.Property(x => x.RenterId).IsRequired().HasColumnName("renter_id");
            b.Property(x => x.StartAt).IsRequired().HasColumnName("start_at");
            b.Property(x => x.DurationHours).IsRequired().HasColumnType("time").HasColumnName("duration_hours");

            b.HasOne<Bicycle>()
             .WithMany()
             .HasForeignKey(x => x.BicycleId)
             .OnDelete(DeleteBehavior.Restrict);

            b.HasOne<Renter>()
             .WithMany()
             .HasForeignKey(x => x.RenterId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        var seed = new BicycleRentalDataSeed();

        modelBuilder.Entity<BicycleModel>().HasData(seed.BicycleModels);
        modelBuilder.Entity<Bicycle>().HasData(seed.Bicycles);
        modelBuilder.Entity<Renter>().HasData(seed.Renters);
        modelBuilder.Entity<Rental>().HasData(seed.Rentals);
    }
}
