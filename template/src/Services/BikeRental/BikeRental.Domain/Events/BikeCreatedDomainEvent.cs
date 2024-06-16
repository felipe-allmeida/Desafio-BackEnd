using BikeRental.Domain.Models.BikeAggregate;
using MediatR;

namespace BikeRental.Domain.Events
{
    public class BikeCreatedDomainEvent : INotification
    {
        public BikeCreatedDomainEvent(Bike bike)
        {
            Bike = bike;
        }

        public Bike Bike { get; }
    }
}
