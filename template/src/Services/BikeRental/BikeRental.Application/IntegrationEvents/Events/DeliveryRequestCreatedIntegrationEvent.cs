using BikeRental.CrossCutting.EventBus.Events;

namespace BikeRental.Application.IntegrationEvents.Events
{
    public record DeliveryRequestCreatedIntegrationEvent : IntegrationEvent
    {
        public DeliveryRequestCreatedIntegrationEvent(Guid deliveryRequestId, int status, int priceCents, DateTimeOffset createdAt)
        {
            DeliveryRequestId = deliveryRequestId;
            Status = status;
            PriceCents = priceCents;
            CreatedAt = createdAt;
        }

        public Guid DeliveryRequestId { get; init; }
        public int Status { get; init; }
        public int PriceCents { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
    }
}
