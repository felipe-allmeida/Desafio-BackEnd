using BikeRental.CrossCutting.EventBus.Events;

namespace BikeRental.Application.IntegrationEvents.Events
{
    public record BikeCreatedIntegrationEvent : IntegrationEvent
    {
        public BikeCreatedIntegrationEvent(long bikeId, string plate, int year, string model)
        {
            BikeId = bikeId;
            Plate = plate;
            Model = model;
            Year = year;
        }

        public long BikeId { get; init; }
        public string Plate { get; init; }
        public int Year { get; init; }
        public string Model { get; init; }
    }
}
