using BikeRental.CrossCutting.EventBus.Events;

namespace BikeRental.Application.IntegrationEvents.Events
{
    public record DeliveryRiderCreatedIntegrationEvent : IntegrationEvent
    {
        public DeliveryRiderCreatedIntegrationEvent(long deliveryRiderId, string name, string cnpj, DateTimeOffset birthday)
        {
            DeliveryRiderId = deliveryRiderId;
            Name = name;
            Cnpj = cnpj;
            Birthday = birthday;
        }

        public long DeliveryRiderId { get; init; }
        public string Name { get; init; }
        public string Cnpj { get; init; }
        public DateTimeOffset Birthday { get; init; }
    }
}
