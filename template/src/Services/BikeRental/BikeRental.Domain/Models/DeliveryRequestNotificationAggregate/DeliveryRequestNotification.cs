using BikeRental.Domain.Enums;
using BikeRental.Domain.Events;
using BikeRental.Domain.Exceptions;
using BuildingBlocks.Common;

namespace BikeRental.Domain.Models.DeliveryRequestNotificationAggregate
{
    public class DeliveryRequestNotification : AggregateRoot<Guid>
    {
        protected DeliveryRequestNotification() : base()
        {

        }

        public DeliveryRequestNotification(Guid deliveryRequestId, long deliveryRiderId) : this()
        {
            Status = EDeliveryRequestNotificationStatus.Pending;
            DeliveryRequestId = deliveryRequestId;
            DeliveryRiderId = deliveryRiderId;
            AddDomainEvent(new DeliveryRequestNotificationCreatedDomainEvent(this));
        }
        public EDeliveryRequestNotificationStatus Status { get; private set; }

        public Guid DeliveryRequestId { get; private set; }
        public long DeliveryRiderId { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }

        public void MarkAsConsumed()
        {
            if (Status == EDeliveryRequestNotificationStatus.Consumed)
                throw new DomainException("Delivery request notification is already consumed.");

            Status = EDeliveryRequestNotificationStatus.Consumed;
        }
    }
}
