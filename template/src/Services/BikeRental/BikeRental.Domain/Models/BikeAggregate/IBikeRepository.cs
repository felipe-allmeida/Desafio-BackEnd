using BuildingBlocks.Common;

namespace BikeRental.Domain.Models.BikeAggregate
{
    public interface IBikeRepository : IRepository<Bike>
    {
        Task<Bike> GetByIdAsync(long id);
        Task<bool> ExistsByPlateAsync(string plate);
    }
}
