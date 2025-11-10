using AutoMapper;
using BicycleRental.Application.Contracts.BicycleModels;
using BicycleRental.Application.Contracts.Bicycles;
using BicycleRental.Application.Contracts.Rentals;
using BicycleRental.Application.Contracts.Renters;
using BicycleRental.Domain.Models;

namespace BicycleRental.Application.Mapping;

/// <summary>
/// AutoMapper profile for mapping domain models to DTOs and back.
/// </summary>
public class BicycleRentalProfile : Profile
{
    public BicycleRentalProfile()
    {
        CreateMap<BicycleModel, BicycleModelDto>();
        CreateMap<BicycleModelCreateUpdateDto, BicycleModel>();

        CreateMap<Bicycle, BicycleDto>();
        CreateMap<BicycleCreateUpdateDto, Bicycle>();

        CreateMap<Renter, RenterDto>();
        CreateMap<RenterCreateUpdateDto, Renter>();

        CreateMap<Rental, RentalDto>();
        CreateMap<RentalCreateUpdateDto, Rental>();
    }
}
