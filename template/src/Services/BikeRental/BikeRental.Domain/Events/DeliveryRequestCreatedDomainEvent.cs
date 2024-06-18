using BikeRental.Domain.Models.DeliveryRequestAggregate;
using MediatR;

namespace BikeRental.Domain.Events
{
    public class DeliveryRequestCreatedDomainEvent : INotification
    {
        public DeliveryRequestCreatedDomainEvent(DeliveryRequest deliveryRequest)
        {
            DeliveryRequest = deliveryRequest;
        }

        public DeliveryRequest DeliveryRequest { get; }
    }
}
