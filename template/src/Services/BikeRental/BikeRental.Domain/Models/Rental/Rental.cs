using BuildingBlocks.Common;

namespace BikeRental.Domain.Models.Rental
{
    public class Rental : AggregateRoot<Guid>
    {
        public long BikeId { get; private set; }
        public long DeliveryRiderId { get; private set; }

        public DateTimeOffset StartAt { get; private set; }
        public DateTimeOffset EndAt { get; private set; }
        public DateTimeOffset ExpectedReturnAt { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset? UpdatedAt { get; private set; }

    }
}
