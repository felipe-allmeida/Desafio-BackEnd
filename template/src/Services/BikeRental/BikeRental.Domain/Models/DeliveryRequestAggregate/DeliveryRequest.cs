using BikeRental.Domain.Enums;
using BikeRental.Domain.Events;
using BikeRental.Domain.Exceptions;
using BuildingBlocks.Common;

namespace BikeRental.Domain.Models.DeliveryRequestAggregate
{
    public class DeliveryRequest : AggregateRoot<Guid>
    {
        protected DeliveryRequest() : base()
        {

        }

        public DeliveryRequest(int priceCents) : this()
        {
            if (priceCents <= 0)
                throw new DomainException("Price must be greater than 0.");
            Status = EDeliveryRequestStatus.Avaiable;
            PriceCents = priceCents;

            AddDomainEvent(new DeliveryRequestCreatedDomainEvent(this));
        }

        public long? DeliveryRiderId { get; private set; }
        public EDeliveryRequestStatus Status { get; private set; }
        public int PriceCents { get; private set; }
        public DateTimeOffset CreatedAt { get; private set; }
        public DateTimeOffset UpdatedAt { get; private set; }

        public void MarkAsAccepted(long deliveryRiderId)
        {
            if (Status != EDeliveryRequestStatus.Avaiable)
                throw new DomainException("Delivery request is not available.");
            DeliveryRiderId = deliveryRiderId;
            Status = EDeliveryRequestStatus.Accepted;
        }

        public void MarkAsDelivered()
        {
            if (Status != EDeliveryRequestStatus.Accepted)
                throw new DomainException("Delivery request is not accepted.");
            Status = EDeliveryRequestStatus.Delivered;
        }
    }
}
