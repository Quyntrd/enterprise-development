using BicycleRental.Domain.Enums;
using BicycleRental.Domain.DataSeed;

namespace BicycleRental.Tests;

/// <summary>
/// Unit tests for the BikeRental domain using a prepopulated deterministic data seed.
/// The fixture provides BicycleModels, Bicycles, Renters and Rentals for LINQ-based assertions.
/// </summary>
public class BicycleRentalTests(BicycleRentalDataSeed dataSeed) : IClassFixture<BicycleRentalDataSeed>
{
    /// <summary>
    /// Ensures the fixture contains 2 sport models.
    /// </summary>
    [Fact]
    public void SportBicycles_Count_ShouldMatchSeed()
    {
        var sportModelIds = dataSeed.BicycleModels.Where(m => m.Type == BicycleType.Sport).Select(m => m.Id).ToHashSet();

        var sportBicyclesCount = dataSeed.Bicycles.Count(b => sportModelIds.Contains(b.ModelId));
        Assert.Equal(2, sportBicyclesCount);
    }

    /// <summary>
    /// Calculates profit per model (DurationHours * PricePerHourAtRental), returns top 5 models by profit,
    /// and verifies the result is ordered descending by profit and contains at most five items.
    /// </summary>
    [Fact]
    public void GetTop5ModelsByProfit_ShouldReturnAtMostFiveOrderedDescending()
    {
        var profitByModel = (from r in dataSeed.Rentals
                             join b in dataSeed.Bicycles on r.BicycleId equals b.Id
                             join m in dataSeed.BicycleModels on b.ModelId equals m.Id
                             group new { r, m } by m.Id into g
                             select new
                             {
                                 ModelId = g.Key,
                                 Profit = g.Sum(x => x.m.PricePerHour * (decimal)x.r.DurationHours.TotalHours)
                             })
                            .OrderByDescending(x => x.Profit)
                            .Take(5)
                            .ToList();

        Assert.NotNull(profitByModel);
        Assert.True(profitByModel.Count <= 5);

        var isOrdered = profitByModel.SequenceEqual(profitByModel.OrderByDescending(x => x.Profit));
        Assert.True(isOrdered, "profitByModel should be ordered by descending profit");
    }

    /// <summary>
    /// Returns top 5 models by total rental duration (sum of hours) and verifies ordering descending by total hours.
    /// </summary>
    [Fact]
    public void GetTop5ModelsByDuration_ShouldReturnAtMostFiveOrderedDescending()
    {
        var durationByModel = (from r in dataSeed.Rentals
                               join b in dataSeed.Bicycles on r.BicycleId equals b.Id
                               join m in dataSeed.BicycleModels on b.ModelId equals m.Id
                               group r by m.Id into g
                               select new { ModelId = g.Key, Total = g.Aggregate(TimeSpan.Zero, (acc, x) => acc + x.DurationHours) })
                              .OrderByDescending(x => x.Total)
                              .Take(5)
                              .ToList();

        Assert.NotNull(durationByModel);
        Assert.True(durationByModel.Count <= 5);

        var isOrdered = durationByModel.SequenceEqual(durationByModel.OrderByDescending(x => x.Total));
        Assert.True(isOrdered, "durationByModel should be ordered by descending total duration");
    }

    /// <summary>
    /// Verifies basic statistical invariants for rental durations: min, max, average.
    /// Ensures values are non-negative and min <= avg <= max.
    /// </summary>
    [Fact]
    public void RentalDurationMinMaxAverage_ShouldBeConsistent()
    {
        Assert.NotEmpty(dataSeed.Rentals);

        var min = dataSeed.Rentals.Min(r => r.DurationHours.Hours);
        var max = dataSeed.Rentals.Max(r => r.DurationHours.Hours);
        var avg = dataSeed.Rentals.Average(r => r.DurationHours.Hours);

        Assert.True(min >= 0, "min must be non-negative");
        Assert.True(max >= 0, "max must be non-negative");
        Assert.True(min <= avg && avg <= max, "min <= avg <= max should hold");
    }

    /// <summary>
    /// Sums rental time by bicycle type and asserts that the sum across types equals the overall total rental hours.
    /// </summary>
    [Fact]
    public void SumRentalTimePerType_TotalEqualsOverallTotal()
    {
        var totalsByType = (from r in dataSeed.Rentals
                            join b in dataSeed.Bicycles on r.BicycleId equals b.Id
                            join m in dataSeed.BicycleModels on b.ModelId equals m.Id
                            group r.DurationHours by m.Type into g
                            select new { Type = g.Key, Total = g.Aggregate(TimeSpan.Zero, (acc, x) => acc + x) })
                           .ToDictionary(x => x.Type, x => x.Total);

        var sumAcrossTypes = totalsByType.Values.Aggregate(TimeSpan.Zero, (acc, t) => acc + t);
        var total = dataSeed.Rentals.Aggregate(TimeSpan.Zero, (acc, r) => acc + r.DurationHours);

        Assert.Equal(total, sumAcrossTypes);
    }

    /// <summary>
    /// Identifies models that were never rented and asserts that expected model ids exist among them.
    /// </summary>
    [Fact]
    public void ModelsNeverRented_ShouldContainExpectedModels()
    {
        var rentedModelIds = dataSeed.Rentals
            .Join(dataSeed.Bicycles, r => r.BicycleId, b => b.Id, (r, b) => b.ModelId)
            .Distinct()
            .ToHashSet();

        var neverRented = dataSeed.BicycleModels.Where(m => !rentedModelIds.Contains(m.Id)).ToList();

        Assert.Contains(neverRented, m => m.Id == 11);
        Assert.Contains(neverRented, m => m.Id == 12);
    }

    /// <summary>
    /// Finds the renter with the highest rental count and asserts the expected top renter id and count.
    /// </summary>
    [Fact]
    public void TopRentersByCount_ShouldReturnExpectedTopRenter()
    {
        var top = dataSeed.Rentals.GroupBy(r => r.RenterId).Select(g => new { RenterId = g.Key, Count = g.Count() }).OrderByDescending(x => x.Count).First();

        Assert.Equal(1, top.RenterId);
        Assert.Equal(4, top.Count);
    }

    /// <summary>
    /// Asserts that all bicycle serial numbers in the fixture are unique.
    /// </summary>
    [Fact]
    public void Bicycles_SerialNumber_AreUnique()
    {
        Assert.Equal(dataSeed.Bicycles.Count, dataSeed.Bicycles.Select(b => b.SerialNumber).Distinct().Count());
    }
}
