using BicycleRental.Domain.Enums;
using BicycleRental.Domain.Models;

namespace BicycleRental.Domain.Fixtures;

/// <summary>
///
/// </summary>
public class BicycleRentalFixture
{
    /// <summary>
    ///
    /// </summary>
    public List<BicycleModel> BicycleModels { get; } = [];

    /// <summary>
    ///
    /// </summary>
    public List<Bicycle> Bicycles { get; } = [];

    /// <summary>
    /// 
    /// </summary>
    public List<Renter> Renters { get; } = [];

    /// <summary>
    /// 
    /// </summary>
    public List<Rental> Rentals { get; } = new();

    private int _rentalId = 1;

    public BicycleRentalFixture()
    {
        BicycleModels.AddRange([
            new BicycleModel
            {
                Id = 1,
                Name = "SportPro 1000",
                Type = BicycleType.Sport,
                WheelSize = 28,
                MaxPassengerWeightKg = 100m,
                WeightKg = 8.5m,
                BrakeType = "Disc",
                ModelYear = 2024,
                PricePerHour = 7.50m
            },
            new BicycleModel
            {
                Id = 2,
                Name = "SpeedX Race",
                Type = BicycleType.Sport,
                WheelSize = 28,
                MaxPassengerWeightKg = 95m,
                WeightKg = 7.8m,
                BrakeType = "Disc",
                ModelYear = 2023,
                PricePerHour = 9.00m
            },
            new BicycleModel
            {
                Id = 3,
                Name = "MountainMax",
                Type = BicycleType.Mountain,
                WheelSize = 27,
                MaxPassengerWeightKg = 120m,
                WeightKg = 14.0m,
                BrakeType = "Disc",
                ModelYear = 2022,
                PricePerHour = 5.50m
            },
            new BicycleModel
            {
                Id = 4,
                Name = "CityComfort",
                Type = BicycleType.City,
                WheelSize = 26,
                MaxPassengerWeightKg = 110m,
                WeightKg = 12.5m,
                BrakeType = "V-Brake",
                ModelYear = 2021,
                PricePerHour = 3.50m
            },
            new BicycleModel
            {
                Id = 5,
                Name = "EcoRide E1",
                Type = BicycleType.Electric,
                WheelSize = 26,
                MaxPassengerWeightKg = 120m,
                WeightKg = 22.0m,
                BrakeType = "Disc",
                ModelYear = 2024,
                PricePerHour = 12.00m
            },
            new BicycleModel
            {
                Id = 6,
                Name = "RoadMaster",
                Type = BicycleType.Road,
                WheelSize = 28,
                MaxPassengerWeightKg = 90m,
                WeightKg = 8.9m,
                BrakeType = "Caliper",
                ModelYear = 2020,
                PricePerHour = 6.00m
            },
            new BicycleModel
            {
                Id = 7,
                Name = "TrailRunner",
                Type = BicycleType.Mountain,
                WheelSize = 27,
                MaxPassengerWeightKg = 115m,
                WeightKg = 13.0m,
                BrakeType = "Disc",
                ModelYear = 2023,
                PricePerHour = 5.75m
            },
            new BicycleModel
            {
                Id = 8,
                Name = "UrbanLite",
                Type = BicycleType.City,
                WheelSize= 26,
                MaxPassengerWeightKg = 105m,
                WeightKg = 11.0m,
                BrakeType = "V-Brake",
                ModelYear = 2022,
                PricePerHour = 3.00m
            },
            new BicycleModel
            {
                Id = 9,
                Name = "SprintElite",
                Type = BicycleType.Road,
                WheelSize = 28,
                MaxPassengerWeightKg = 95m,
                WeightKg = 7.6m,
                BrakeType = "Caliper",
                ModelYear = 2025,
                PricePerHour = 8.50m
            },
            new BicycleModel
            {
                Id = 10,
                Name = "ComfortCity",
                Type = BicycleType.City,
                WheelSize = 26,
                MaxPassengerWeightKg = 120m,
                WeightKg = 13.5m,
                BrakeType = "V-Brake",
                ModelYear = 2019,
                PricePerHour = 2.50m
            },
            new BicycleModel
            {
                Id = 11,
                Name = "VintageTour",
                Type = BicycleType.City,
                WheelSize = 26,
                MaxPassengerWeightKg = 100m,
                WeightKg = 15.0m,
                BrakeType = "Coaster",
                ModelYear = 2015,
                PricePerHour = 1.50m
            },
            new BicycleModel
            {
                Id = 12,
                Name = "TestModelX",
                Type = BicycleType.Mountain,
                WheelSize = 27,
                MaxPassengerWeightKg = 110m,
                WeightKg = 14.5m,
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
                FirstName = "Иван",
                LastName = "Иванов",
                Patronymic = "Иванович",
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
                Patronymic = "Андреевна",
                Phone = "+7-900-000-0004"
            },
            new Renter
            {
                Id = 5,
                FirstName = "Анна",
                LastName = "Анникова",
                Patronymic = "Михайловна",
                Phone = "+7-900-000-0005"
            },
            new Renter
            {
                Id = 6,
                FirstName = "Елена",
                LastName = "Еленова",
                Patronymic = null,
                Phone = "+7-900-000-0006"
            },
            new Renter
            {
                Id = 7,
                FirstName = "Дмитрий",
                LastName = "Дмитриев",
                Patronymic = "Владимирович",
                Phone = "+7-900-000-0007"
            },
            new Renter
            {
                Id = 8,
                FirstName = "Михаил",
                LastName = "Михайлов",
                Patronymic = "Павлович",
                Phone = "+7-900-000-0008"
            },
            new Renter
            {
                Id = 9,
                FirstName = "Татьяна",
                LastName = "Татьяничева",
                Patronymic = null,
                Phone = "+7-900-000-0009"
            },
            new Renter
            {
                Id = 10,
                FirstName = "Алексей",
                LastName = "Алексеев",
                Patronymic = "Николаевич",
                Phone = "+7-900-000-0010"
            },
            new Renter
            {
                Id = 11,
                FirstName = "Ирина",
                LastName = "Ирикова",
                Patronymic = "Петровна",
                Phone = "+7-900-000-0011"
            },
            new Renter
            {
                Id = 12,
                FirstName = "Владимир",
                LastName = "Владимиров",
                Patronymic = "Сергеевич",
                Phone = "+7-900-000-0012"
            }
        ]);

        CreateRental(bicycleId: 1,  renterId: 1, startAt: new DateTime(2025, 1, 10, 9, 0, 0),  durationHours: 2.5m);
        CreateRental(bicycleId: 2, renterId: 1, startAt: new DateTime(2025, 1, 12, 14, 0, 0), durationHours: 1.0m);
        CreateRental(bicycleId: 1, renterId: 1, startAt: new DateTime(2025, 2, 3, 16, 30, 0), durationHours: 3.0m);
        CreateRental(bicycleId: 3, renterId: 2, startAt: new DateTime(2025, 2, 5, 10, 0, 0), durationHours: 1.5m);
        CreateRental(bicycleId: 4, renterId: 3, startAt: new DateTime(2025, 2, 7, 11, 30, 0), durationHours: 4.0m);
        CreateRental(bicycleId: 5, renterId: 4, startAt: new DateTime(2025, 2, 10, 9, 15, 0), durationHours: 0.5m);
        CreateRental(bicycleId: 6, renterId: 5, startAt: new DateTime(2025, 3, 1, 8, 0, 0), durationHours: 6.0m);
        CreateRental(bicycleId: 7, renterId: 6, startAt: new DateTime(2025, 3, 4, 18, 0, 0), durationHours: 2.0m);
        CreateRental(bicycleId: 8, renterId: 7, startAt: new DateTime(2025, 3, 10, 12, 0, 0), durationHours: 12.0m);
        CreateRental(bicycleId: 9, renterId: 8, startAt: new DateTime(2025, 3, 15, 7, 0, 0), durationHours: 8.0m);
        CreateRental(bicycleId: 2, renterId: 9, startAt: new DateTime(2025, 4, 1, 9, 0, 0), durationHours: 3.5m);
        CreateRental(bicycleId: 3, renterId: 10, startAt: new DateTime(2025, 4, 3, 14, 0, 0), durationHours: 1.0m);
        CreateRental(bicycleId: 5, renterId: 11, startAt: new DateTime(2025, 4, 10, 10, 0, 0), durationHours: 24.0m);
        CreateRental(bicycleId: 9, renterId: 1, startAt: new DateTime(2025, 4, 12, 9, 30, 0), durationHours: 0.75m);
    }
    /// <summary>
    /// 
    /// </summary>
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


