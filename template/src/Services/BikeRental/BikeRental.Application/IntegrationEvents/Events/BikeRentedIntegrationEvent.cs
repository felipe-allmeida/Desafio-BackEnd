using BikeRental.CrossCutting.EventBus.Events;
using BikeRental.Domain.Enums;

namespace BikeRental.Application.IntegrationEvents.Events
{
    public record BikeRentedIntegrationEvent : IntegrationEvent
    {
        public BikeRentedIntegrationEvent(Guid rentalId, long bikeId, long deliveryRiderId, ERentalStatus status, DateTimeOffset createdAt, DateTimeOffset startAt, DateTimeOffset endAt, DateTimeOffset expectedReturnAt, int dailyPriceCents, int priceCents, int? penaltyPriceCents)
        {
            RentalId = rentalId;
            BikeId = bikeId;
            DeliveryRiderId = deliveryRiderId;
            Status = status;
            CreatedAt = createdAt;
            StartAt = startAt;
            EndAt = endAt;
            ExpectedReturnAt = expectedReturnAt;
            DailyPriceCents = dailyPriceCents;
            PriceCents = priceCents;
            PenaltyPriceCents = penaltyPriceCents;
        }

        public Guid RentalId { get; init; }
        public long BikeId { get; init; }
        public long DeliveryRiderId { get; init; }
        public ERentalStatus Status { get; init; }
        public DateTimeOffset CreatedAt { get; init; }
        public DateTimeOffset StartAt { get; private set; }

        public DateTimeOffset EndAt { get; private set; }
        public DateTimeOffset ExpectedReturnAt { get; private set; }
        public int DailyPriceCents { get; private set; }
        public int PriceCents { get; private set; }
        public int? PenaltyPriceCents { get; private set; }
    }
}
