using AutoMapper;
using BicycleRental.Api.Contracts.BicycleModels;
using BicycleRental.Api.Contracts.Bicycles;
using BicycleRental.Api.Contracts.Rentals;
using BicycleRental.Api.Contracts.Renters;
using BicycleRental.Domain.Models;

namespace BicycleRental.Application.Mapping;

/// <summary>
/// AutoMapper profile for mapping domain models to DTOs and back.
/// </summary>
public class BicycleRentalProfile : Profile
{
    public BicycleRentalProfile()
    {
        // BicycleModel
        CreateMap<BicycleModel, BicycleModelDto>();
        CreateMap<BicycleModelCreateUpdateDto, BicycleModel>();

        // Bicycle
        CreateMap<Bicycle, BicycleDto>();
        CreateMap<BicycleCreateUpdateDto, Bicycle>();

        // Renter
        CreateMap<Renter, RenterDto>();
        CreateMap<RenterCreateUpdateDto, Renter>();

        // Rental
        CreateMap<Rental, RentalDto>();
        CreateMap<RentalCreateUpdateDto, Rental>();
    }
}
