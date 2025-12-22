namespace BicycleRental.Wasm.Api;

public class BicycleRentalApiWrapper
{
    private readonly BicycleRentalClient _client;

    public BicycleRentalApiWrapper(HttpClient httpClient, IConfiguration configuration)
    {
        var baseUrl = configuration["services:bicyclerental-api-host:https:0"];
        _client = new BicycleRentalClient(baseUrl, httpClient);
    }

    public Task<ICollection<BicycleModelDto>> GetAllBicycleModels() => _client.BicycleModelsAllAsync();
    public Task<BicycleModelDto> GetBicycleModel(int id) => _client.BicycleModelsGETAsync(id);
    public Task<BicycleModelDto> CreateBicycleModel(BicycleModelCreateUpdateDto dto) => _client.BicycleModelsPOSTAsync(dto);
    public Task<BicycleModelDto> UpdateBicycleModel(int id, BicycleModelCreateUpdateDto dto) => _client.BicycleModelsPUTAsync(id, dto);
    public Task DeleteBicycleModel(int id) => _client.BicycleModelsDELETEAsync(id);

    public Task<ICollection<BicycleDto>> GetBicyclesByModel(int modelId) => _client.BicyclesAsync(modelId);

    public Task<ICollection<BicycleDto>> GetAllBicycles() => _client.BicyclesAllAsync();
    public Task<BicycleDto> GetBicycle(int id) => _client.BicyclesGETAsync(id);
    public Task<BicycleDto> CreateBicycle(BicycleCreateUpdateDto dto) => _client.BicyclesPOSTAsync(dto);
    public Task<BicycleDto> UpdateBicycle(int id, BicycleCreateUpdateDto dto) => _client.BicyclesPUTAsync(id, dto);
    public Task DeleteBicycle(int id) => _client.BicyclesDELETEAsync(id);

    public Task<ICollection<RentalDto>> GetBicycleRentals(int bicycleId) => _client.RentalsAsync(bicycleId);

    public Task<ICollection<RenterDto>> GetAllRenters() => _client.RentersAllAsync();
    public Task<RenterDto> GetRenter(int id) => _client.RentersGETAsync(id);
    public Task<RenterDto> CreateRenter(RenterCreateUpdateDto dto) => _client.RentersPOSTAsync(dto);
    public Task<RenterDto> UpdateRenter(int id, RenterCreateUpdateDto dto) => _client.RentersPUTAsync(id, dto);
    public Task DeleteRenter(int id) => _client.RentersDELETEAsync(id);

    public Task<ICollection<RentalDto>> GetRenterRentals(int renterId) => _client.Rentals2Async(renterId);
    public Task<ICollection<RentalDto>> GetAllRentals() => _client.RentalsAllAsync();
    public Task<RentalDto> GetRental(int id) => _client.RentalsGETAsync(id);
    public Task<RentalDto> CreateRental(RentalCreateUpdateDto dto) => _client.RentalsPOSTAsync(dto);
    public Task<RentalDto> UpdateRental(int id, RentalCreateUpdateDto dto) => _client.RentalsPUTAsync(id, dto);
    public Task DeleteRental(int id) => _client.RentalsDELETEAsync(id);
}
