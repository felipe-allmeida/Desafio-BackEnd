using BuildingBlocks.Common;

namespace BikeRental.Domain.Models.DeliveryRiderAggregate
{
    public interface IDeliveryRiderQueryRepository : IQueryRepository<DeliveryRider>
    {
        Task<List<DeliveryRider>> GetAvaiableDeliveryRiders();
    }
}
