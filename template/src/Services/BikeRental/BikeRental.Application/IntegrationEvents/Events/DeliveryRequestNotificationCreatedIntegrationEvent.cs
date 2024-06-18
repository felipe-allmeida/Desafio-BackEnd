using BikeRental.CrossCutting.EventBus.Events;

namespace BikeRental.Application.IntegrationEvents.Events
{
    public record DeliveryRequestNotificationCreatedIntegrationEvent : IntegrationEvent
    {
        public DeliveryRequestNotificationCreatedIntegrationEvent(Guid deliveryRequestId, long deliveryRiderId)
        {
            DeliveryRequestId = deliveryRequestId;
            DeliveryRiderId = deliveryRiderId;
        }

        public Guid DeliveryRequestId { get; init; }
        public long DeliveryRiderId { get; init; }
    }
}
