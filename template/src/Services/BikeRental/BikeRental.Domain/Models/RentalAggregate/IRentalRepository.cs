using BuildingBlocks.Common;

namespace BikeRental.Domain.Models.RentalAggregate
{
    public interface IRentalRepository : IRepository<Rental>
    {
        Task<Rental?> GetByIdAsync(long deliveryRiderId, Guid id);
        Task<bool> ExistsRentalForBike(long bikeId);
    }
}
