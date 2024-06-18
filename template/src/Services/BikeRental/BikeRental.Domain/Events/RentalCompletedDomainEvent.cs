using BikeRental.Domain.Models.RentalAggregate;
using MediatR;

namespace BikeRental.Domain.Events
{
    public class RentalCompletedDomainEvent : INotification
    {
        public RentalCompletedDomainEvent(Rental rental)
        {
            Rental = rental;
        }

        public Rental Rental { get; }
    }
}
