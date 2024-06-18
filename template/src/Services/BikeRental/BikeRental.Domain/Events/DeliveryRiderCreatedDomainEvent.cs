using BikeRental.Domain.Models.DeliveryRiderAggregate;
using MediatR;

namespace BikeRental.Domain.Events
{
    public class DeliveryRiderCreatedDomainEvent : INotification
    {
        public DeliveryRiderCreatedDomainEvent(DeliveryRider deliveryRider)
        {
            DeliveryRider = deliveryRider;
        }

        public DeliveryRider DeliveryRider { get; }
    }
}
