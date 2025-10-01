using BicycleRental.Domain.Enums;
using BicycleRental.Domain.Fixtures;

namespace BicycleRental.Tests;

/// <summary>
/// Unit tests for the BikeRental domain using a prepopulated deterministic fixture.
/// The fixture provides BicycleModels, Bicycles, Renters and Rentals for LINQ-based assertions.
/// </summary>
public class BicycleRentalTests(BicycleRentalFixture fixture) : IClassFixture<BicycleRentalFixture>
{
    /// <summary>
    /// Verifies that all returned bicycles belong to models of type sport bicycle.
    /// Ensures the fixture contains sport models and that bicycles reference those models.
    /// </summary>
    [Fact]
    public void GetAllSportBicycles_ShouldReturnOnlySportModels()
    {
        var sportModelIds = fixture.BicycleModels
            .Where(m => m.Type == BicycleType.Sport)
            .Select(m => m.Id)
            .ToHashSet();

        var sportBicycles = fixture.Bicycles
            .Where(b => sportModelIds.Contains(b.ModelId))
            .ToList();

        Assert.NotEmpty(sportModelIds);
        Assert.All(sportBicycles, b => Assert.Contains(b.ModelId, sportModelIds));
    }

    /// <summary>
    /// Calculates profit per model (DurationHours * PricePerHourAtRental), returns top 5 models by profit,
    /// and verifies the result is ordered descending by profit and contains at most five items.
    /// </summary>
    [Fact]
    public void GetTop5ModelsByProfit_ShouldReturnAtMostFiveOrderedDescending()
    {
        var profitByModel = fixture.Rentals
            .Join(fixture.Bicycles, r => r.BicycleId, b => b.Id, (r, b) => new { r, b })
            .Join(fixture.BicycleModels, rb => rb.b.ModelId, m => m.Id, (rb, m) => new
            {
                ModelId = m.Id,
                Profit = rb.r.DurationHours * rb.r.PricePerHourAtRental
            })
            .GroupBy(x => x.ModelId)
            .Select(g => new { ModelId = g.Key, Profit = g.Sum(x => x.Profit) })
            .OrderByDescending(x => x.Profit)
            .Take(5)
            .ToList();

        Assert.NotNull(profitByModel);
        Assert.True(profitByModel.Count <= 5);

        for (var i = 1; i < profitByModel.Count; i++)
        {
            Assert.True(profitByModel[i - 1].Profit >= profitByModel[i].Profit,
                $"Model {profitByModel[i - 1].ModelId} should have >= profit than {profitByModel[i].ModelId}");
        }

        if (profitByModel.Any())
        {
            var maxProfit = profitByModel.Max(x => x.Profit);
            Assert.Equal(maxProfit, profitByModel.First().Profit);
        }
    }

    /// <summary>
    /// Returns top 5 models by total rental duration (sum of hours) and verifies ordering descending by total hours.
    /// </summary>
    [Fact]
    public void GetTop5ModelsByDuration_ShouldReturnAtMostFiveOrderedDescending()
    {
        var durationByModel = fixture.Rentals
            .Join(fixture.Bicycles, r => r.BicycleId, b => b.Id, (r, b) => new { r, b })
            .Join(fixture.BicycleModels, rb => rb.b.ModelId, m => m.Id, (rb, m) => new
            {
                ModelId = m.Id,
                Hours = rb.r.DurationHours
            })
            .GroupBy(x => x.ModelId)
            .Select(g => new { ModelId = g.Key, TotalHours = g.Sum(x => x.Hours) })
            .OrderByDescending(x => x.TotalHours)
            .Take(5)
            .ToList();

        Assert.NotNull(durationByModel);
        Assert.True(durationByModel.Count <= 5);

        for (var i = 1; i < durationByModel.Count; i++)
        {
            Assert.True(durationByModel[i - 1].TotalHours >= durationByModel[i].TotalHours,
                $"Model {durationByModel[i - 1].ModelId} should have >= total hours than {durationByModel[i].ModelId}");
        }
    }

    /// <summary>
    /// Verifies basic statistical invariants for rental durations: min, max, average.
    /// Ensures values are non-negative and min <= avg <= max.
    /// </summary>
    [Fact]
    public void RentalDurationMinMaxAverage_ShouldBeConsistent()
    {
        Assert.NotEmpty(fixture.Rentals);

        var min = fixture.Rentals.Min(r => r.DurationHours);
        var max = fixture.Rentals.Max(r => r.DurationHours);
        var avg = fixture.Rentals.Average(r => r.DurationHours);

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
        var totalsByType = fixture.Rentals
            .Join(fixture.Bicycles, r => r.BicycleId, b => b.Id, (r, b) => new { r, b })
            .Join(fixture.BicycleModels, rb => rb.b.ModelId, m => m.Id, (rb, m) => new { rb.r.DurationHours, m.Type })
            .GroupBy(x => x.Type)
            .ToDictionary(g => g.Key, g => g.Sum(x => x.DurationHours));

        var sumAcrossTypes = totalsByType.Values.Sum();
        var total = fixture.Rentals.Sum(r => r.DurationHours);

        Assert.Equal(total, sumAcrossTypes);
    }

    /// <summary>
    /// Identifies models that were never rented and asserts that expected model ids exist among them (fixture-specific check).
    /// </summary>
    [Fact]
    public void ModelsNeverRented_ShouldContainExpectedModels()
    {
        var rentedModelIds = fixture.Rentals
            .Join(fixture.Bicycles, r => r.BicycleId, b => b.Id, (r, b) => b.ModelId)
            .Distinct()
            .ToHashSet();

        var neverRented = fixture.BicycleModels.Where(m => !rentedModelIds.Contains(m.Id)).ToList();

        Assert.Contains(neverRented, m => m.Id == 11);
        Assert.Contains(neverRented, m => m.Id == 12);
    }

    /// <summary>
    /// Finds the renter with the highest rental count and asserts the expected top renter id and count (fixture-specific).
    /// </summary>
    [Fact]
    public void TopRentersByCount_ShouldReturnExpectedTopRenter()
    {
        var top = fixture.Rentals
            .GroupBy(r => r.RenterId)
            .Select(g => new { RenterId = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .First();

        Assert.Equal(1, top.RenterId);
        Assert.Equal(4, top.Count);
    }

    /// <summary>
    /// Ensures that each rental's <see cref="Rental.PricePerHourAtRental"/> equals the corresponding model's current price in the fixture.
    /// This guarantees revenue calculations are based on the recorded price at rental time.
    /// </summary>
    [Fact]
    public void AllRentalsPricePerHourAtRental_MatchesModelPrice()
    {
        var mismatches = fixture.Rentals
            .Join(fixture.Bicycles, r => r.BicycleId, b => b.Id, (r, b) => new { r, b })
            .Join(fixture.BicycleModels, rb => rb.b.ModelId, m => m.Id, (rb, m) => new { rb.r, ModelPrice = m.PricePerHour })
            .Where(x => x.r.PricePerHourAtRental != x.ModelPrice)
            .ToList();

        Assert.Empty(mismatches);
    }

    /// <summary>
    /// Verifies that all rentals have a non-default date time in <see cref="Rental.StartAt"/>.
    /// </summary>
    [Fact]
    public void AllRentals_StartAt_IsInitialized()
    {
        Assert.NotEmpty(fixture.Rentals);
        Assert.All(fixture.Rentals, r => Assert.NotEqual(default(DateTime), r.StartAt));
    }

    /// <summary>
    /// Asserts that all bicycle serial numbers in the fixture are unique.
    /// </summary>
    [Fact]
    public void Bicycles_SerialNumber_AreUnique()
    {
        var dup = fixture.Bicycles.GroupBy(b => b.SerialNumber)
                   .Where(g => g.Count() > 1)
                   .Select(g => new { Serial = g.Key, Count = g.Count() })
                   .ToList();
        Assert.Empty(dup);
    }
}
