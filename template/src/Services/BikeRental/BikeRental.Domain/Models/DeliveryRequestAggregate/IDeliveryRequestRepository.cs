using BikeRental.Domain.Models.DeliveryRequestNotificationAggregate;
using BuildingBlocks.Common;

namespace BikeRental.Domain.Models.DeliveryRequestAggregate
{
    public interface IDeliveryRequestRepository : IRepository<DeliveryRequest>
    {
        Task<DeliveryRequest?> GetByIdAsync(Guid id);
    }
}
