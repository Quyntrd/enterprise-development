using BicycleRental.Domain.Enums;
using BicycleRental.Domain.Fixtures;

namespace BicycleRental.Tests;

/// <summary>
///
/// </summary>
public class BikeRentalTests(BicycleRentalFixture fixture) : IClassFixture<BicycleRentalFixture>
{
    /// <summary>
    ///
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
    ///
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
    ///
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
    ///
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
    ///
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
    ///
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
    ///
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
    /// 
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
    /// 
    /// </summary>
    [Fact]
    public void AllRentals_StartAt_IsInitialized()
    {
        Assert.NotEmpty(fixture.Rentals);
        Assert.All(fixture.Rentals, r => Assert.NotEqual(default(DateTime), r.StartAt));
    }
    /// <summary>
    /// 
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
