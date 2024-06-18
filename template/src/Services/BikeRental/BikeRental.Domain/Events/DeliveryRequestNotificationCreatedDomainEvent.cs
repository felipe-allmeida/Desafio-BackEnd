using BikeRental.Domain.Models.DeliveryRequestNotificationAggregate;
using MediatR;

namespace BikeRental.Domain.Events
{
    public class DeliveryRequestNotificationCreatedDomainEvent : INotification
    {
        public DeliveryRequestNotificationCreatedDomainEvent(DeliveryRequestNotification notification)
        {
            Notification = notification;
        }

        public DeliveryRequestNotification Notification { get; }
    }
}
