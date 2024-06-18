using BuildingBlocks.Common;

namespace BikeRental.Domain.Models.BikeAggregate
{
    public interface IBikeRepository : IRepository<Bike>
    {
        Task<bool> BikeHasRentals(long id);
        Task<Bike?> GetByIdAsync(long id);
        Task<bool> ExistsByPlateAsync(string plate);        
    }
}
