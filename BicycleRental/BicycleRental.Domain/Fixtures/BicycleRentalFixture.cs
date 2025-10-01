using BicycleRental.Domain.Enums;
using BicycleRental.Domain.Models;

namespace BicycleRental.Domain.Fixtures;

/// <summary>
/// Test Fixture for testing the BicycleRental.Domain.
/// Initializes:
/// BicycleModels, Bicycles, Renter, Rental classes.
/// Used in xUnit tests.
/// </summary>
public class BicycleRentalFixture
{
    /// <summary>
    /// List of <see cref="BicycleModel"/>.
    /// </summary>
    public List<BicycleModel> BicycleModels { get; } = [];

    /// <summary>
    /// List of <see cref="Bicycle"/>.
    /// </summary>
    public List<Bicycle> Bicycles { get; } = [];

    /// <summary>
    /// List of <see cref="Renter"/>.
    /// </summary>
    public List<Renter> Renters { get; } = [];

    /// <summary>
    /// List of <see cref="Rental"/>.
    /// </summary>
    public List<Rental> Rentals { get; } = [];

    private int _rentalId = 1;

    /// <summary>
    /// Constructor initializes fixture with determinastic test data.
    /// Adds <see cref="BicycleModel"/>, <see cref="Bicycle"/>, <see cref="Renter"/>, <see cref="Rental"/>.
    /// </summary>
    public BicycleRentalFixture()
    {
        BicycleModels.AddRange([
            new BicycleModel
            {
                Id = 1,
                Name = "SportPro 1000",
                Type = BicycleType.Sport,
                WheelSizeInInches = 28,
                MaxPassengerWeightKg = 100,
                WeightKg = 8.5,
                BrakeType = "Disc",
                ModelYear = 2024,
                PricePerHour = 7.50m
            },
            new BicycleModel
            {
                Id = 2,
                Name = "SpeedX Race",
                Type = BicycleType.Sport,
                WheelSizeInInches = 28,
                MaxPassengerWeightKg = 95,
                WeightKg = 7.8,
                BrakeType = "Disc",
                ModelYear = 2023,
                PricePerHour = 9.00m
            },
            new BicycleModel
            {
                Id = 3,
                Name = "MountainMax",
                Type = BicycleType.Mountain,
                WheelSizeInInches = 27,
                MaxPassengerWeightKg = 120,
                WeightKg = 14.0,
                BrakeType = "Disc",
                ModelYear = 2022,
                PricePerHour = 5.50m
            },
            new BicycleModel
            {
                Id = 4,
                Name = "CityComfort",
                Type = BicycleType.City,
                WheelSizeInInches = 26,
                MaxPassengerWeightKg = 110,
                WeightKg = 12.5,
                BrakeType = "V-Brake",
                ModelYear = 2021,
                PricePerHour = 3.50m
            },
            new BicycleModel
            {
                Id = 5,
                Name = "EcoRide E1",
                Type = BicycleType.Electric,
                WheelSizeInInches = 26,
                MaxPassengerWeightKg = 120,
                WeightKg = 22.0,
                BrakeType = "Disc",
                ModelYear = 2024,
                PricePerHour = 12.00m
            },
            new BicycleModel
            {
                Id = 6,
                Name = "RoadMaster",
                Type = BicycleType.Road,
                WheelSizeInInches = 28,
                MaxPassengerWeightKg = 90,
                WeightKg = 8.9,
                BrakeType = "Caliper",
                ModelYear = 2020,
                PricePerHour = 6.00m
            },
            new BicycleModel
            {
                Id = 7,
                Name = "TrailRunner",
                Type = BicycleType.Mountain,
                WheelSizeInInches = 27,
                MaxPassengerWeightKg = 115,
                WeightKg = 13.0,
                BrakeType = "Disc",
                ModelYear = 2023,
                PricePerHour = 5.75m
            },
            new BicycleModel
            {
                Id = 8,
                Name = "UrbanLite",
                Type = BicycleType.City,
                WheelSizeInInches= 26,
                MaxPassengerWeightKg = 105,
                WeightKg = 11.0,
                BrakeType = "V-Brake",
                ModelYear = 2022,
                PricePerHour = 3.00m
            },
            new BicycleModel
            {
                Id = 9,
                Name = "SprintElite",
                Type = BicycleType.Road,
                WheelSizeInInches = 28,
                MaxPassengerWeightKg = 95,
                WeightKg = 7.6,
                BrakeType = "Caliper",
                ModelYear = 2025,
                PricePerHour = 8.50m
            },
            new BicycleModel
            {
                Id = 10,
                Name = "ComfortCity",
                Type = BicycleType.City,
                WheelSizeInInches = 26,
                MaxPassengerWeightKg = 120,
                WeightKg = 13.5,
                BrakeType = "V-Brake",
                ModelYear = 2019,
                PricePerHour = 2.50m
            },
            new BicycleModel
            {
                Id = 11,
                Name = "VintageTour",
                Type = BicycleType.City,
                WheelSizeInInches = 26,
                MaxPassengerWeightKg = 100,
                WeightKg = 15.0,
                BrakeType = "Coaster",
                ModelYear = 2015,
                PricePerHour = 1.50m
            },
            new BicycleModel
            {
                Id = 12,
                Name = "TestModelX",
                Type = BicycleType.Mountain,
                WheelSizeInInches = 27,
                MaxPassengerWeightKg = 110,
                WeightKg = 14.5,
                BrakeType = "Disc",
                ModelYear = 2018,
                PricePerHour = 4.00m
            }
        ]);

        Bicycles.AddRange([
            new Bicycle
            {
                Id = 1,
                SerialNumber = "SN-1001",
                ModelId = 1,
                Color = "Красный"
            },
            new Bicycle
            {
                Id = 2,
                SerialNumber = "SN-1002",
                ModelId = 2,
                Color = "Чёрный"
            },
            new Bicycle
            {
                Id = 3,
                SerialNumber = "SN-1003",
                ModelId = 3,
                Color = "Зелёный"
            },
            new Bicycle
            {
                Id = 4,
                SerialNumber = "SN-1004",
                ModelId = 4,
                Color = "Синий"
            },
            new Bicycle
            {
                Id = 5,
                SerialNumber = "SN-1005",
                ModelId = 5,
                Color = "Белый"
            },
            new Bicycle
            {
                Id = 6,
                SerialNumber = "SN-1006",
                ModelId = 6,
                Color = "Красный"
            },
            new Bicycle
            {
                Id = 7,
                SerialNumber = "SN-1007",
                ModelId = 7,
                Color = "Чёрный"
            },
            new Bicycle
            {
                Id = 8,
                SerialNumber = "SN-1008",
                ModelId = 8,
                Color = "Серый"
            },
            new Bicycle
            {
                Id = 9,
                SerialNumber = "SN-1009",
                ModelId = 9,
                Color = "Оранжевый"
            },
            new Bicycle
            {
                Id = 10,
                SerialNumber = "SN-1010",
                ModelId = 10,
                Color = "Жёлтый"
            },
            new Bicycle
            {
                Id = 11,
                SerialNumber = "SN-1011",
                ModelId = 11,
                Color = "Коричневый"
            },
            new Bicycle
            {
                Id = 12,
                SerialNumber = "SN-1012",
                ModelId = 12,
                Color = "Бирюзовый"
            }
        ]);

        Renters.AddRange([
            new Renter
            {
                Id = 1,
                FirstName = "Евгений",
                LastName = "Баженов",
                Patronymic = "Владимирович",
                Phone = "+7-900-000-0001"
            },
            new Renter
            {
                Id = 2,
                FirstName = "Пётр",
                LastName = "Петров",
                Patronymic = "Петрович",
                Phone = "+7-900-000-0002"
            },
            new Renter
            {
                Id = 3,
                FirstName = "Сергей",
                LastName = "Сергеев",
                Patronymic = "Сергеевич",
                Phone = "+7-900-000-0003"
            },
            new Renter
            {
                Id = 4,
                FirstName = "Ольга",
                LastName = "Ольгина",
                Patronymic = "Олеговна",
                Phone = "+7-900-000-0004"
            },
            new Renter
            {
                Id = 5,
                FirstName = "Бутс",
                LastName = "Клозович",
                Patronymic = "Моторсайклов",
                Phone = "+7-900-000-0005"
            },
            new Renter
            {
                Id = 6,
                FirstName = "Евгений",
                LastName = "Батиков",
                Patronymic = null,
                Phone = "+7-900-000-0006"
            },
            new Renter
            {
                Id = 7,
                FirstName = "Станислав",
                LastName = "Васильев",
                Patronymic = "Александрович",
                Phone = "+7-900-000-0007"
            },
            new Renter
            {
                Id = 8,
                FirstName = "Ружье",
                LastName = "Бондарчука",
                Patronymic = "Михалкович",
                Phone = "+7-900-000-0008"
            },
            new Renter
            {
                Id = 9,
                FirstName = "Алексей",
                LastName = "Команданте",
                Patronymic = null,
                Phone = "+7-900-000-0009"
            },
            new Renter
            {
                Id = 10,
                FirstName = "Алексей",
                LastName = "Алексеев",
                Patronymic = "Алексеевич",
                Phone = "+7-900-000-0010"
            },
            new Renter
            {
                Id = 11,
                FirstName = "Бикукл",
                LastName = "Бикукле",
                Patronymic = "Бикуклович",
                Phone = "+7-900-000-0011"
            },
            new Renter
            {
                Id = 12,
                FirstName = "Михал",
                LastName = "Оскар",
                Patronymic = "Наградович",
                Phone = "+7-900-000-0012"
            }
        ]);

        CreateRental(1, 1, new DateTime(2025, 1, 10, 9, 0, 0), 2.5m);
        CreateRental(2, 1, new DateTime(2025, 1, 12, 14, 0, 0), 1.0m);
        CreateRental(1, 1, new DateTime(2025, 2, 3, 16, 30, 0), 3.0m);
        CreateRental(3, 2, new DateTime(2025, 2, 5, 10, 0, 0), 1.5m);
        CreateRental(4, 3, new DateTime(2025, 2, 7, 11, 30, 0), 4.0m);
        CreateRental(5, 4, new DateTime(2025, 2, 10, 9, 15, 0), 0.5m);
        CreateRental(6, 5, new DateTime(2025, 3, 1, 8, 0, 0), 6.0m);
        CreateRental(7, 6, new DateTime(2025, 3, 4, 18, 0, 0), 2.0m);
        CreateRental(8, 7, new DateTime(2025, 3, 10, 12, 0, 0), 12.0m);
        CreateRental(9, 8, new DateTime(2025, 3, 15, 7, 0, 0), 8.0m);
        CreateRental(2, 9, new DateTime(2025, 4, 1, 9, 0, 0), 3.5m);
        CreateRental(3, 10, new DateTime(2025, 4, 3, 14, 0, 0), 1.0m);
        CreateRental(5, 11, new DateTime(2025, 4, 10, 10, 0, 0), 24.0m);
        CreateRental(9, 1, new DateTime(2025, 4, 12, 9, 30, 0), 0.75m);
    }
    /// <summary>
    /// Helper to create a <see cref="Rental"/> and add it to <see cref="Rentals"/>.
    /// Copies price of <see cref="BicycleModel"/> at creation time.
    /// </summary>
    /// <param name="bicycleId">Id of the bicycle being rented.</param>
    /// <param name="renterId">Id of the renter (client).</param>
    /// <param name="startAt">Start time of rental.</param>
    /// <param name="durationHours">Duration of rental in hours.</param>
    private void CreateRental(int bicycleId, int renterId, DateTime startAt, decimal durationHours)
    {
        var bike = Bicycles.SingleOrDefault(b => b.Id == bicycleId)
            ?? throw new InvalidOperationException($"Bicycle Id={bicycleId} not found in fixture.");

        var model = BicycleModels.SingleOrDefault(m => m.Id == bike.ModelId)
            ?? throw new InvalidOperationException($"Model Id={bike.ModelId} not found for Bicycle Id={bicycleId}.");

        var rental = new Rental
        {
            Id = _rentalId++,
            BicycleId = bicycleId,
            RenterId = renterId,
            StartAt = startAt,
            DurationHours = durationHours,
            PricePerHourAtRental = model.PricePerHour
        };

        Rentals.Add(rental);
    }
}


